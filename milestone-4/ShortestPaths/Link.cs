using System;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace ShortestPaths
{
  internal class Link
  {
    public Network Network { get; private set; }
    public Node FromNode { get; private set; }
    public Node ToNode { get; private set; }
    public int Cost { get; private set; }

    public Link(Network network, Node fromNode, Node toNode, int cost)
    {
      Network = network;
      FromNode = fromNode;
      ToNode = toNode;
      Cost = cost;

      FromNode.AddLink(this);
      Network.AddLink(this);
    }
    public override string ToString()
    {
      return $"{FromNode} --> {ToNode} ({Cost})";
    }

    public Brush TextBrush { get; set; } = Brushes.Black;

    public Brush Stroke { get; set; } = Brushes.Green;

    public double StrokeThickness { get; set; } = 2.0;

    public void Draw(Canvas canvas)
    {
      canvas.DrawLine(FromNode.Center, ToNode.Center, Stroke, StrokeThickness);
    }

    public const double RADIUS = 10;

    public void DrawLabel(Canvas canvas)
    {
      Vector d = ToNode.Center - FromNode.Center;
      double angle = Math.Atan2(d.Y, d.X) * 180 / Math.PI;

      Point c = FromNode.Center + d / 3;

      canvas.DrawEllipse(c.CenteredBounds(RADIUS), Brushes.White, Brushes.White, 0);
      canvas.DrawString(Cost.ToString(), 2 * RADIUS, 2 * RADIUS, c, angle, 12, TextBrush);
    }
  }
}
