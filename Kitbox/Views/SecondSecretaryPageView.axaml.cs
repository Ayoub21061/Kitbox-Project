using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class SecondSecretaryPageView : Window
    {
        public SecondSecretaryPageView()
        {
            InitializeComponent();
            DataContext = new SecretaryViewModel(); 
        }
    }
}