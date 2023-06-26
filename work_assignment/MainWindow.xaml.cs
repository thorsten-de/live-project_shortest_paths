using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace WorkAssignments;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Network MyNetwork = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            var dialog =
                new OpenFileDialog
                {
                    DefaultExt = ".jobs",
                    Filter = "Network Files|*.jobs|All Files|*.*"
                };
            if (dialog.ShowDialog() == true) 
                MyNetwork.LoadJobsFile(dialog.FileName, mainCanvas, Console.Out);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
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
}