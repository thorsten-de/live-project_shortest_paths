using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var network = new Network();
      var a = new Node(network, new Point(20, 20), "A");
      var b = new Node(network, new Point(120, 20), "B");
      var c = new Node(network, new Point(70, 120), "C");

      new Link(network, a, b, 100);
      var l = new Link(network, b, c, 130);
      new Link(network, a, c, 50);

      Debug.WriteLine(a);
      Debug.WriteLine(l);
      Debug.WriteLine(network.Serialize());
    }
  }
}
