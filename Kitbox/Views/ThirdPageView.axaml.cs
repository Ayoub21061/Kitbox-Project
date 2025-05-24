using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class ThirdPageView : Window
    {
        public ThirdPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();

        }
    }
}
