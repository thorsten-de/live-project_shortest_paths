using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace ShortestPaths
{
  public enum AlgorithmType
  {
    LabelSetting,
    LabelCorrecting
  }

    
  internal class Network { 
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
        .AppendJoin("\n", Nodes.Select(n => string.Format("{0:F0},{1:F0},{2}", n.Center.X, n.Center.Y, n.Text)))
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
          new Node(this, new Point(double.Parse(nodeData[0]), double.Parse(nodeData[1])), nodeData[2]);
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
      bool drawLabels = Nodes.Count < 100;
      
      Rect bounds = GetBounds();
      canvas.Width = bounds.Width + MARGIN;
      canvas.Height = bounds.Height + MARGIN;

      foreach (var link in Links) link.Draw(canvas);

      if (drawLabels)
      {
        foreach (var link in Links) link.DrawLabel(canvas);
      }

      foreach (var node in Nodes) node.Draw(canvas, drawLabels);
    }

    public Rect GetBounds() =>
      Nodes.Aggregate(
        new Rect(0, 0, 0, 0),
        (bounds, node) =>
        Rect.Union(bounds, new Rect(node.Center, new Point(0, 0)))
      );

    public Node StartNode { get; set; }
    
    public Node EndNode { get; set; }

    internal void node_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (((FrameworkElement)sender).Tag is Node node)
      {
        OnNodeClicked(node, e);
      }
    }

    protected void OnNodeClicked(Node node, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed)
      {
        if (StartNode != null)
        {
          StartNode.IsStartNode = false;
        }
        node.IsStartNode = true;
        StartNode = node;
        CheckForPath();
      }
      if (e.RightButton == MouseButtonState.Pressed)
      {
        if (EndNode != null)
        {
          EndNode.IsEndNode = false;
        }
        node.IsEndNode = true;
        EndNode = node;
        CheckForPath();
      }
    }

    private PathAlgorithm _pathAlgorithm = PathAlgorithms.LabelSetting;

    /// <summary>
    /// Path Algorithm Strategy
    /// </summary>
    public PathAlgorithm AlgorithmType {
      get => _pathAlgorithm;
      set
      {
        _pathAlgorithm = value;
        CheckForPath();
      }
    }
    public void initPathTree()
    {
      Links.ForEach(l => l.IsInTree = l.IsInPath = false);
      foreach (var n in Nodes)
      {
        n.TotalCost = double.PositiveInfinity;
        n.ShortestPathLink = null;
      }
      StartNode.TotalCost = 0;
    }


    public void CheckForPath()
    {
      if (StartNode == null)
        return;

      initPathTree();
      _pathAlgorithm.FindPathTree(this);

      if (StartNode != null & EndNode != null)
      {
        FindPath();
      }
    }

    public void FindPath()
    {
      var node = EndNode;
      while (node != StartNode)
      {
        node.ShortestPathLink.IsInPath = true;
        node = node.ShortestPathLink.FromNode;
      }
      Debug.WriteLine("FindPath: Cost {0}", EndNode.TotalCost);
    }
  }
}
