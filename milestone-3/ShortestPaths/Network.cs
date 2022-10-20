using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows;
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

    // I prefer aptly named static methods over constructor overloads
    public static Network FromFile(string filename)
    {
      var network = new Network();
      network.ReadFromFile(filename);
      return network;
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
        .AppendJoin("\n", Nodes.Select(n => string.Format("{0},{1},{2}", n.Center.X, n.Center.Y, n.Text)))
        .AppendLine("\n# Links.")
        .AppendJoin("\n", Links.Select(l => string.Format("{0},{1},{2}", l.FromNode.Index, l.ToNode.Index, l.Cost)))
      .ToString();

    public void SaveToFile(string filename) => 
      File.WriteAllText(filename, Serialize());

    public void Deserialize(string serialized)
    {
      Clear();
      using(var reader = new StringReader(serialized))
      {
        int nodeCount = int.Parse(ReadNextLine(reader));
        int linkCount = int.Parse(ReadNextLine(reader));
        for (int i = 0; i < nodeCount; i++)
        {
          var nodeData = ReadNextLine(reader).Split(','); 
          new Node(this, new System.Windows.Point(double.Parse(nodeData[0]), double.Parse(nodeData[1])), nodeData[2]);
        }
        for (int i = 0; i < linkCount; i++)
        {
          var linkData = ReadNextLine(reader).Split(',');
          Node from = Nodes[int.Parse(linkData[0])];
          Node to = Nodes[int.Parse(linkData[1])];
          new Link(this, from, to, int.Parse(linkData[2]));
        }
      }
    }

    private string? ReadNextLine(StringReader reader)
    {
      string? line;
      do
      {
        line = reader.ReadLine();
        if (line == null) break;
        line = line.Split('#').FirstOrDefault()?.Trim();
      }
      while (string.IsNullOrEmpty(line));

      return line;
    }

    public void ReadFromFile(string filename) =>
      Deserialize(File.ReadAllText(filename));


    const double MARGIN = 20;
    public void Draw(Canvas canvas)
    {
      Rect bounds = GetBounds();
      canvas.Width = bounds.Width + MARGIN;
      canvas.Height = bounds.Height + MARGIN;

      foreach (var link in Links) link.Draw(canvas);

      foreach (var link in Links) link.DrawLabel(canvas);

      foreach (var node in Nodes) node.Draw(canvas);
    }

    public Rect GetBounds() =>
      Nodes.Aggregate(
        new Rect(0, 0, 0, 0),
        (bounds, node) =>
        Rect.Union(bounds, new Rect(node.Center, new Point(0, 0)))
      );

  }
}
