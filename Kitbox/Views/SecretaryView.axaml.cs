using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kitbox.ViewModels;
//hello


namespace Kitbox.Views
{
    public partial class SecretaryView : UserControl
{
    public SecretaryView()
    {
        InitializeComponent();
        DataContext = new SecretaryViewModel(); 
    }
}
}
