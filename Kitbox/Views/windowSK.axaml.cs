using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
public partial class WindowSK : Window
{
    public WindowSK()
    {
        InitializeComponent();
        DataContext = new SkViewModel();
    }
    
}
}