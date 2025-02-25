using Avalonia.Controls;
using Avalonia.Input;  // Assure-toi d'ajouter cette ligne
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
