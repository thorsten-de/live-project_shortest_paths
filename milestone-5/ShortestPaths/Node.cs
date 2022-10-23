using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShortestPaths
{
  internal class Node
  {
    public int Index { get; internal set; }
    public Network Network { get; private set; }
    public Point Center { get; private set; }
    public string Text { get; set; }
    public IList<Link> Links { get; private set; }

    internal double TotalCost { get; set; }
    internal Link? ShortestPathLink { get; set; }

    public Node(Network network, Point center, string text)
    {
      Links = new List<Link>();
      Network = network;
      Center = center;
      Text = text;
      Index = -1;
      Network.AddNode(this);
    }

    public override string ToString()
    {
      return $"[{Text}]";
    }

    public void AddLink(Link link) => Links.Add(link);


    public Brush Stroke { get; set; } = Brushes.Black;
    public double StrokeThickness { get; set; } = 2.0;

    public Brush Background { get; set; } = Brushes.White;
    public Brush Foregrond { get; set; } = Brushes.SteelBlue;

    public const double LARGE_RADIUS = 10;
    public const double SMALL_RADIUS = 3;

    public void Draw(Canvas canvas, bool drawLabels)
    {
      double radius = drawLabels ? LARGE_RADIUS : SMALL_RADIUS;
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

    private Ellipse MyEllipse { get; set; }
    private Label MyLabel { get; set; }

    private bool isStartNode = false;
    public bool IsStartNode
    {
      get => isStartNode;
      set
      {
        isStartNode = value;
        SetNodeAppearance();
      }
    }

    private bool isEndNode = false;
    public bool IsEndNode
    {
      get => isEndNode;
      set
      {
        isEndNode = value;
        SetNodeAppearance();
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
      else
      if (isEndNode)
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
}
