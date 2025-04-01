using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new SecretaryViewModel();
    }
}