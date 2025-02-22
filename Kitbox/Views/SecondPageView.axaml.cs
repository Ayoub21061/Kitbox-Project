using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class SecondPageView : Window
    {
        public SecondPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(); 
        }
    }
}
