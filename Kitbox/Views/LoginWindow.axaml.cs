
using Avalonia.Controls;

using Kitbox.ViewModels;

namespace Kitbox.Views
{
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
    
}
}