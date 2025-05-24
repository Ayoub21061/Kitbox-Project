using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class FifthPageView : Window
    {
        public FifthPageView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }
    }
}