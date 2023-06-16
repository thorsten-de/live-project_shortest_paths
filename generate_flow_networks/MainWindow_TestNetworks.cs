using System;
using System.Windows;

namespace FlowNetworks;

partial class MainWindow
{
    public static double MARGIN = 10;

    private void MakeRandomizedLink(Random rand, Network network, Node from, Node to)
    {
        new Link(network, from, to, rand.Next(1, 6));
    }

    private Network BuildGridNetwork(string filename, double width, double height, int numRows, int numCols)
    {
        var network = new Network();
        var bounds = new Rect(0, 0, width, height);
        bounds.Inflate(-MARGIN, -MARGIN);
        var stepX = bounds.Width / (numCols - 1);
        var stepY = bounds.Height / (numRows - 1);
        var rand = new Random();


        for (var y = 0; y < numRows; y++)
        for (var x = 0; x < numCols; x++)
            new Node(network, bounds.TopLeft + new Vector(x * stepX, y * stepY), (network.Nodes.Count + 1).ToString());

        foreach (var node in network.Nodes)
        {
            var row = node.Index / numCols;
            var col = node.Index % numCols;

            if (col < numCols - 1) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index + 1]);
            if (row < numRows - 1) MakeRandomizedLink(rand, network, node, network.Nodes[node.Index + numCols]);
        }

        network.SaveToFile(filename);

        return network;
    }

    private void Generate_3x3_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("3x3_test.net", 300, 300, 3, 3);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }
    
    private void Generate_4x4_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("4x4_test.net", 300, 300, 4, 4);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }
    
    private void Generate_5x8_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("5x8_test.net", 600, 400, 5, 8);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }

    private void Generate_6x10_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("6x10_test.net", 600, 400, 6, 10);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }

    private void Generate_10x15_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("10x15_test.net", 600, 400, 10, 15);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }

    private void Generate_20x30_Click(object sender, RoutedEventArgs e)
    {
        MyNetwork = BuildGridNetwork("20x30_test.net", 600, 400, 20, 30);
        algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
        DrawNetwork();
    }
}