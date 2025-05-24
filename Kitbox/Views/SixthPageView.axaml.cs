using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class SixthPageView : Window
    {
        public SixthPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }
    }
}