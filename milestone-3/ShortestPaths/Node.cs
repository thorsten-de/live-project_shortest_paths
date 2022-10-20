﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace ShortestPaths
{
  internal class Node
  {
    public int Index { get; internal set; }
    public Network Network { get; private set; }
    public Point Center { get; private set; }
    public string Text { get; private set; }
    public IList<Link> Links { get; private set; }

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
      canvas.DrawEllipse(Center.CenteredBounds(radius), Background, Stroke, StrokeThickness);
      if (drawLabels)
        canvas.DrawString(Text, 2 * radius, 2 * radius, Center, 0, 12, Foregrond);
    }

  }
}
