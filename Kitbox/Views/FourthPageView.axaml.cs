using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class FourthPageView : Window
    {
        public FourthPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }
    }
}