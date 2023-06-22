using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace FlowNetworks;

internal class Link
{
    private const double RADIUS = 10;

    private bool isInPath;

    private bool isInTree;
    private readonly Network _network;
    private readonly Node _fromNode;
    private readonly Node _toNode;
    private readonly double _capacity;
    private double _flow;
    private Brush _textBrush = Brushes.Black;
    private Brush _stroke = Brushes.Silver;
    private double _strokeThickness = 2;
    private Line _myLine;

    public Link(Network network, Node fromNode, Node toNode, int capacity)
    {
        _network = network;
        _fromNode = fromNode;
        _toNode = toNode;
        _capacity = capacity;
        Flow = 0;

        FromNode.AddLink(this);
        ToNode.AddBacklink(this);
        Network.AddLink(this);
    }

    public Network Network => _network;

    public Node FromNode => _fromNode;

    public Node ToNode => _toNode;

    public double Capacity => _capacity;

    public double Flow
    {
        get => _flow;
        set
        {
            _flow = value; 
            SetLinkAppearance();
        }
        
    }

    public void UseRegularFrom(Node from)
    {
        VisitNode(ToNode, from);
    }

    public void UseBacklinkFrom(Node from)
    {
        VisitNode(FromNode, from);
        IsBacklink = true;
    }

    private void VisitNode(Node target, Node from)
    {
        target.FromNode = from;
        target.FromLink = this;
        target.Visited = true;
    }

    public void AlterFlow(double delta) =>
        Flow += IsBacklink ? -delta : +delta;

    public double ResidualCapacity => 
        Capacity - Flow;

    public double Delta => IsBacklink ? Flow : ResidualCapacity;

    public bool IsBacklink { get; set; }
    
    public Brush TextBrush
    {
        get => _textBrush;
        set => _textBrush = value;
    }

    public Brush Stroke
    {
        get => _stroke;
        set => _stroke = value;
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set => _strokeThickness = value;
    }

    private Line MyLine
    {
        get => _myLine;
        set => _myLine = value;
    }

    public bool IsInTree
    {
        get => isInTree;
        set
        {
            isInTree = value;
            SetLinkAppearance();
        }
    }

    public bool IsInPath
    {
        get => isInPath;
        set
        {
            isInPath = value;
            SetLinkAppearance();
        }
    }

    public override string ToString()
    {
        return $"{FromNode} --> {ToNode} ({Flow}/{Capacity}){(IsBacklink ? "*": "")}";
    }
    
    public void Draw(Canvas canvas)
    {
        canvas.DrawLine(FromNode.Center, ToNode.Center, Stroke, 2 * Capacity);
        MyLine = canvas.DrawLine(FromNode.Center, ToNode.Center, Stroke, StrokeThickness);
    }

    public void DrawLabel(Canvas canvas)
    {
        var d = ToNode.Center - FromNode.Center;
        var angle = Math.Atan2(d.Y, d.X) * 180 / Math.PI;

        var c = FromNode.Center + d / 3;

        canvas.DrawEllipse(c.CenteredBounds(RADIUS), Brushes.White, Brushes.White, 0);
        MyLabel = canvas.DrawString(Display, 2 * RADIUS, 2 * RADIUS, c, angle, 12, TextBrush);
    }

    private string Display => $"{Flow}/{Capacity}";

    private Label MyLabel;

    private void SetLinkAppearance()
    {
        if (MyLine == null) return;

        if (Flow > 0)
        {
            MyLine.Stroke = Brushes.Red;
            MyLine.StrokeThickness = 2 * Flow;
        }
        else
        {
            MyLine.Stroke = Stroke;
            MyLine.StrokeThickness = StrokeThickness;
        }

        if (MyLabel != null)
        {
            MyLabel.Content = Display;
        }
    }
}