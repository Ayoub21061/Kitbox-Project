using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace Kitbox.Views
{
    public partial class EmptyView : UserControl
    {
        public EmptyView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // Méthode appelée lorsque le bouton "OK" est cliqué
        private void OnOkButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Action à effectuer lorsque le bouton "OK" est cliqué
            // Par exemple, on ferme la fenêtre
            
        }

        // Gérer le clic du bouton OK
        
    }
}

