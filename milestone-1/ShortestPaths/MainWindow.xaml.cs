using System.Text;
using System.Windows;

namespace ShortestPaths
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();


    }



    void ValidateNetwork(Network network, string filename)
    {
      string serializedOriginal = network.Serialize();
      network.SaveToFile(filename);

      network.ReadFromFile(filename);
      string serializedReloaded = network.Serialize();

      bool isMatch = serializedOriginal == serializedReloaded;

      statusLabel.Content = isMatch ? "OK" : "Serializations do not match";
      netTextBox.Text = new StringBuilder(serializedOriginal).Append("\n\n").Append(serializedReloaded).ToString();


    }


    private void validateNetwork1_Click(object sender, RoutedEventArgs e)
    {

      var network = new Network();
      var n = new[] {
      new Node(network, new Point(20, 20), "A"),
      new Node(network, new Point(120, 20), "B"),
      };

      new Link(network, n[0], n[1], 10);

      ValidateNetwork(network, "network_1.net");


    }

    private void validateNetwork2_Click(object sender, RoutedEventArgs e)
    {
      var network = new Network();
      var n = new[] {
      new Node(network, new Point(20, 20), "A"),
      new Node(network, new Point(120, 20), "B"),
      new Node(network, new Point(20, 120), "C"),
      new Node(network, new Point(120, 120), "D")
      };

      new Link(network, n[0], n[1], 10);
      new Link(network, n[1], n[3], 15);
      new Link(network, n[0], n[2], 20);
      new Link(network, n[2], n[3], 25);

      ValidateNetwork(network, "network_2.net");

    }

    private void validateNetwork3_Click(object sender, RoutedEventArgs e)
    {
      var network = new Network();
      var n = new[] {
      new Node(network, new Point(20, 20), "A"),
      new Node(network, new Point(120, 20), "B"),
      new Node(network, new Point(20, 120), "C"),
      new Node(network, new Point(120, 120), "D") 
      };

      new Link(network, n[0], n[1], 10);
      new Link(network, n[1], n[3], 15);
      new Link(network, n[0], n[2], 20);
      new Link(network, n[2], n[3], 25);
      new Link(network, n[1], n[0], 11);
      new Link(network, n[3], n[1], 16);
      new Link(network, n[2], n[0], 21);
      new Link(network, n[3], n[2], 26);

      ValidateNetwork(network, "network_3.net");

    }
  }
}
