using Avalonia;
using Avalonia.Controls;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public partial class Stocks : Window
    {
        public Stocks()
        {
            InitializeComponent();
            DataContext = new StocksViewModel();
        }
    }
}
