using System.Windows;
using ResourceMonitor.ViewModels;
namespace ResourceMonitor.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
    public MainWindow(IMainWindowViewModel MainWindowViewModel)
    {
        InitializeComponent();
        DataContext = MainWindowViewModel;
    }
}