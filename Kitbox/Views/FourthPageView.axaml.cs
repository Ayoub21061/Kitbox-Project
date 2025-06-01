using Avalonia.Controls;
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