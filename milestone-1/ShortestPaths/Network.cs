using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShortestPaths
{
  internal class Network
  {
    public IList<Node> Nodes { get; private set; } 
    public IList<Link> Links { get; private set; }

    public Network()
    {
      Clear();
    }

    public void Clear()
    {
      Nodes = new List<Node>();
      Links = new List<Link>();
    }

    public void AddNode(Node node)
    {
      node.Index = Nodes.Count;
      Nodes.Add(node);
    }

    public void AddLink(Link link) => Links.Add(link);

    public string Serialize() =>
      new StringBuilder()
        .AppendFormat("{0} # Num nodes.\n", Nodes.Count)
        .AppendFormat("{0} # Num links.\n", Links.Count)
        .AppendLine("# Nodes.")
        .AppendJoin("\n",Nodes.Select(n => string.Format("{0},{1},{2}", n.Center.X, n.Center.Y, n.Text)))
        .AppendLine("\n# Links.")
        .AppendJoin("\n", Links.Select(l => string.Format("{0},{1},{2}", l.FromNode.Index, l.ToNode.Index, l.Cost)))
      .ToString();
  }
}
