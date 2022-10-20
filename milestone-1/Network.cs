using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

  }
}
