using Avalonia.Controls;
using Avalonia.Input; 
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }

    }
}
