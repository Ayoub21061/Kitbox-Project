using CommunityToolkit.Mvvm.ComponentModel;


namespace Kitbox.ViewModels
{
    [ObservableObject]
    public partial class Customer : User
    {

        public Customer(string newUsername, string newPassword, string newEmail) // Constructeur qui initialise l'objet Customer avec les valeurs passées en paramètre. Pour créer un client directement avec Id et email.
        {
            this.username = newUsername;
            this.email = newEmail;
            this.password = newPassword;
            this.windowId = 2; // 2 est l'Id de la fenêtre de l'interface graphique pour les clients.
        }
    }
}
