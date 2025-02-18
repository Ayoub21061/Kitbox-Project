using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Mail;


namespace Kitbox.ViewModels
{
    [ObservableObject]
    public partial class Customer : User
    {
        protected string MailAddress;
        public Customer() // Constructeur qui initialise l'objet Customer avec les valeurs passées en paramètre. Pour créer un client directement avec Id et email.
        {
            this.windowId = 2; // 2 est l'Id de la fenêtre de l'interface graphique pour les clients.
        }
    }
}
