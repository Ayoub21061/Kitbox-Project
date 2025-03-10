using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kitbox.ViewModels;

namespace Kitbox.Views
{
    public class SecretaryView : UserControl
    {
        // Le ViewModel est automatiquement lié à la vue si le DataContext est configuré correctement
        public SecretaryView()
        {
            InitializeComponent(); // Initialiser le XAML

            // Assurez-vous que le DataContext est le ViewModel approprié
            this.DataContext = new SecretaryViewModel();
        }

        private void InitializeComponent()
        {
            // Ce code lie le fichier XAML avec le code-behind
            AvaloniaXamlLoader.Load(this);
        }
    }
}

