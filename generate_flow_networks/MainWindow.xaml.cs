using Microsoft.Win32;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FlowNetworks
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      algorithmComboBox.ItemsSource = PathAlgorithms.All;
      algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;
    }

    private Network MyNetwork = new Network();

    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        var dialog =
          new OpenFileDialog()
          {
            DefaultExt = ".net",
            Filter = "Network Files|*.net|All Files|*.*"
          };
        if (dialog.ShowDialog() == true)
        {
          MyNetwork = Network.FromFile(dialog.FileName);
        }

      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        MyNetwork = new Network();
      }


      algorithmComboBox.SelectedItem = MyNetwork.AlgorithmType;

      DrawNetwork();
    }

    private void MenuItemExit_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void DrawNetwork()
    {
      mainCanvas.Children.Clear();
      MyNetwork.Draw(mainCanvas);
    }

    private void algorithmComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e){
      MyNetwork.AlgorithmType = (PathAlgorithm)algorithmComboBox.SelectedItem;
      DrawNetwork();
    }
  }
}
