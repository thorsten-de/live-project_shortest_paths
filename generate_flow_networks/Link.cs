using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace FlowNetworks;

internal class Link
{
    public const double RADIUS = 10;

    private bool isInPath;

    private bool isInTree;

    public Link(Network network, Node fromNode, Node toNode, int capacity)
    {
        Network = network;
        FromNode = fromNode;
        ToNode = toNode;
        Capacity = capacity;
        Flow = 0;

        FromNode.AddLink(this);
        Network.AddLink(this);
    }

    public Network Network { get; }
    public Node FromNode { get; }
    public Node ToNode { get; }
    public double Capacity { get; }
    
    public double Flow { get; private set; }

    public Brush TextBrush { get; set; } = Brushes.Black;

    public Brush Stroke { get; set; } = Brushes.DarkGreen;

    public double StrokeThickness { get; set; } = 1;

    private Line MyLine { get; set; }

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
        return $"{FromNode} --> {ToNode} ({Capacity})";
    }


    public void Draw(Canvas canvas)
    {
        MyLine = canvas.DrawLine(FromNode.Center, ToNode.Center, Stroke, StrokeThickness);
    }

    public void DrawLabel(Canvas canvas)
    {
        var d = ToNode.Center - FromNode.Center;
        var angle = Math.Atan2(d.Y, d.X) * 180 / Math.PI;

        var c = FromNode.Center + d / 3;

        canvas.DrawEllipse(c.CenteredBounds(RADIUS), Brushes.White, Brushes.White, 0);
        canvas.DrawString($"{Flow}/{Capacity}", 2 * RADIUS, 2 * RADIUS, c, angle, 12, TextBrush);
    }

    private void SetLinkAppearance()
    {
        if (MyLine == null) return;

        if (isInPath)
        {
            MyLine.Stroke = Brushes.Red;
            MyLine.StrokeThickness = 6;
        }
        else if (isInTree)
        {
            MyLine.Stroke = Brushes.Lime;
            MyLine.StrokeThickness = 6;
        }
        else
        {
            MyLine.Stroke = Stroke;
            MyLine.StrokeThickness = StrokeThickness;
        }
    }
}