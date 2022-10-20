using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShortestPaths
{
  partial class MainWindow
  {
    public static double MARGIN = 10;

    private void MakeRandomizedLink(Random rand, Network network, Node from, Node to)
    {
      var distance = to.Center - from.Center;
      new Link(network, from, to, Convert.ToInt32((1.0 + 0.2 * rand.NextDouble()) * distance.Length));
    }

    private Network BuildGridNetwork(string filename, double width, double height, int numRows, int numCols)
    {
      Network network = new Network();
      Rect bounds = new Rect(0, 0, width, height);
      bounds.Inflate(-MARGIN, -MARGIN);
      double stepX = bounds.Width / (numCols - 1);
      double stepY = bounds.Height / (numRows - 1);
      var rand = new Random();


      for (int y = 0; y < numRows; y++) {
        for (int  x = 0; x < numCols; x++) {
          new Node(network, new Point(x * stepX, y * stepY), (network.Nodes.Count + 1).ToString());
        }
      }

      foreach (Node node in network.Nodes) {
        int row = node.Index  / numCols;
        int col = node.Index  % numCols;

        if (col > 0) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index - 1]);
        if (col < numCols - 1) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index + 1]);

        if (row > 0) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index - numCols]);
        if (row < numRows - 1) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index + numCols]);
      }


      network.SaveToFile(filename);

      return network;
    }

    private void Generate_6x10_Click(object sender, RoutedEventArgs e)
    {
      MyNetwork = BuildGridNetwork("6x10_test.net", 600, 400, 6, 10);
      DrawNetwork();        
    }
    
    private void Generate_10x15_Click(object sender, RoutedEventArgs e)
    {
      MyNetwork = BuildGridNetwork("10x15_test.net", 600, 400, 10, 15);
      DrawNetwork();        
    }


  }
}
