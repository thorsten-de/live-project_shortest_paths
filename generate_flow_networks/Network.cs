﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlowNetworks;

public enum AlgorithmType
{
    LabelSetting,
    LabelCorrecting
}

internal class Network
{
    private const double MARGIN = 20;

    public Network()
    {
        Clear();
    }

    public IList<Node> Nodes { get; private set; }
    public IList<Link> Links { get; private set; }

    public Node? StartNode { get; set; }

    public Node? EndNode { get; set; }

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

    public void AddLink(Link link)
    {
        Links.Add(link);
    }

    public string Serialize()
    {
        return new StringBuilder()
            .AppendFormat("{0} # Num nodes.\n", Nodes.Count)
            .AppendFormat("{0} # Num links.\n", Links.Count)
            .AppendLine("# Nodes.")
            .AppendJoin("\n", Nodes.Select(n => string.Format("{0:F0},{1:F0},{2}", n.Center.X, n.Center.Y, n.Text)))
            .AppendLine("\n# Links.")
            .AppendJoin("\n",
                Links.Select(l => string.Format("{0},{1},{2:F0}", l.FromNode.Index, l.ToNode.Index, l.Capacity)))
            .ToString();
    }

    public void SaveToFile(string filename)
    {
        File.WriteAllText(filename, Serialize());
    }

    public void Deserialize(string serialized)
    {
        Clear();
        using (var reader = new StringReader(serialized))
        {
            var nodeCount = int.Parse(ReadNextLine(reader));
            var linkCount = int.Parse(ReadNextLine(reader));
            for (var i = 0; i < nodeCount; i++)
            {
                var nodeData = ReadNextLine(reader).Split(',');
                new Node(this, new Point(double.Parse(nodeData[0]), double.Parse(nodeData[1])), nodeData[2]);
            }

            for (var i = 0; i < linkCount; i++)
            {
                var linkData = ReadNextLine(reader).Split(',');
                var from = Nodes[int.Parse(linkData[0])];
                var to = Nodes[int.Parse(linkData[1])];
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
        } while (string.IsNullOrEmpty(line));

        return line;
    }

    public void ReadFromFile(string filename)
    {
        Deserialize(File.ReadAllText(filename));
    }

    public void Draw(Canvas canvas)
    {
        var drawLabels = Nodes.Count < 100;

        var bounds = GetBounds();
        canvas.Width = bounds.Width + MARGIN;
        canvas.Height = bounds.Height + MARGIN;

        foreach (var link in Links) link.Draw(canvas);

        if (drawLabels)
            foreach (var link in Links)
                link.DrawLabel(canvas);

        foreach (var node in Nodes) node.Draw(canvas, drawLabels);
    }

    public Rect GetBounds()
    {
        return Nodes.Aggregate(
            new Rect(0, 0, 0, 0),
            (bounds, node) =>
                Rect.Union(bounds, new Rect(node.Center, new Point(0, 0)))
        );
    }

    internal void node_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (((FrameworkElement)sender).Tag is Node node) OnNodeClicked(node, e);
    }

    protected void OnNodeClicked(Node node, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (StartNode != null) 
                StartNode.IsStartNode = false;

            node.IsStartNode = true;
            StartNode = node;
            CalculateFlow();
        }

        if (e.RightButton == MouseButtonState.Pressed)
        {
            if (EndNode != null) EndNode.IsEndNode = false;

            node.IsEndNode = true;
            EndNode = node;
            CalculateFlow();
        }
    }

    public void CalculateFlow()
    {
        if (StartNode is null || EndNode is null || StartNode == EndNode)
            return;

        #region inner function
        LinkedList<Link>? FindResidualPath()
        {
            LinkedList<Link> path = new();
            foreach (var node in Nodes)
            {
                node.Visited = false;
                node.FromNode = null;
                node.FromLink = null;
            }

            foreach (var link in  Links)
            {
                link.IsBacklink = false;
            }
            
            Queue<Node> queue = new();
            StartNode.Visited = true;
            queue.Enqueue(StartNode);
            while (queue.Any() && !EndNode.Visited)
            {
                var u = queue.Dequeue();
                
                foreach (var link in u.Links
                             .Where(link => !link.ToNode.Visited && link.ResidualCapacity > 0))
                {
                    link.UseRegularFrom(u);
                    queue.Enqueue(link.ToNode);
                }

                foreach (var link in u.Backlinks
                             .Where(link => !link.FromNode.Visited && link.Flow > 0))
                {
                    link.UseBacklinkFrom(u);
                    queue.Enqueue(link.FromNode);
                }
            }

            if (EndNode.FromNode == null)
                return null;
            
            var pathNode = EndNode;
            while (pathNode != StartNode)
            {
                path.AddFirst(pathNode.FromLink);
                pathNode = pathNode.FromNode;
            }
            return path;
        }
        #endregion
        
        foreach (var link in  Links)
            link.Flow = 0;
        
        for (;;)
        {
            var path = FindResidualPath();
            if (path is null)
                break;
            
            double cf = path.Min(p => p.Delta);
            foreach (var link in path)
                link.AlterFlow(cf);
        }
        
        Debug.WriteLine($"Total flow is: {TotalFlow}");
    }
    
    public double TotalFlow =>
        StartNode?.Links.Sum(l => l.Flow) ?? double.NaN;

    
    
}