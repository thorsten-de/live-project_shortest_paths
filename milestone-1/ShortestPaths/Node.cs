using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

  }
}
