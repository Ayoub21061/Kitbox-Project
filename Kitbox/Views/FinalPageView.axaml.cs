using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class FinalPageView : Window
    {
        public FinalPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }
    }
}