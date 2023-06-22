using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkAssignments;

internal class Node
{
    public const double LARGE_RADIUS = 10;
    public const double SMALL_RADIUS = 3;

    private bool isEndNode;

    private bool isStartNode;

    public Node(Network network, Point center, string text)
    {
        
        Backlinks = new LinkedList<Link>();
        Links = new List<Link>();
        Network = network;
        Center = center;
        Text = text;
        Index = -1;
        Network.AddNode(this);
    }

    public int Index { get; internal set; }
    public Network Network { get; }
    public Point Center { get; }
    public string Text { get; set; }
    public IList<Link> Links { get; }

    public Node? FromNode { get; set; }
    public Link? FromLink { get; set; }
    public bool Visited { get; set; }

    public LinkedList<Link> Backlinks { get; }

    internal double TotalCost { get; set; }
    internal Link? ShortestPathLink { get; set; }

    public Brush Stroke { get; set; } = Brushes.Black;
    public double StrokeThickness { get; set; } = 2.0;

    public Brush Background { get; set; } = Brushes.White;
    public Brush Foregrond { get; set; } = Brushes.SteelBlue;

    private Ellipse MyEllipse { get; set; }
    private Label MyLabel { get; set; }

    public bool IsStartNode
    {
        get => isStartNode;
        set
        {
            isStartNode = value;
            SetNodeAppearance();
        }
    }

    public bool IsEndNode
    {
        get => isEndNode;
        set
        {
            isEndNode = value;
            SetNodeAppearance();
        }
    }

    public override string ToString()
    {
        return $"[{Text}]";
    }

    public void AddLink(Link link)
    {
        Links.Add(link);
    }

    public void AddBacklink(Link link)
    {
        Backlinks.AddLast(link);
    }

    public void Draw(Canvas canvas, bool drawLabels)
    {
        var radius = drawLabels ? LARGE_RADIUS : SMALL_RADIUS;
        MyEllipse = canvas.DrawEllipse(Center.CenteredBounds(radius), Background, Stroke, StrokeThickness);
        MyEllipse.Tag = this;
        MyEllipse.MouseDown += Network.node_MouseDown;

        if (drawLabels)
        {
            MyLabel = canvas.DrawString(Text, 2 * radius, 2 * radius, Center, 0, 12, Foregrond);
            MyLabel.Tag = this;
            MyLabel.MouseDown += Network.node_MouseDown;
        }
    }

    public void SetNodeAppearance()
    {
        if (MyEllipse == null) return;

        if (isStartNode)
        {
            MyEllipse.Fill = Brushes.Pink;
            MyEllipse.Stroke = Brushes.Red;
            MyEllipse.StrokeThickness = 2;
        }
        else if (isEndNode)
        {
            MyEllipse.Fill = Brushes.LightGreen;
            MyEllipse.Stroke = Brushes.Green;
            MyEllipse.StrokeThickness = 2;
        }
        else
        {
            MyEllipse.Fill = Brushes.White;
            MyEllipse.Stroke = Brushes.Black;
            MyEllipse.StrokeThickness = 1;
        }
    }
}