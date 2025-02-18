using CommunityToolkit.Mvvm.ComponentModel;

namespace Kitbox.Models
{
    public partial class Customer : ObservableObject
    {
        [ObservableProperty]
        private string id;

        [ObservableProperty]
        private string email;
        public Customer() // Constructeur par défaut qui initialise Id et Email avec string.Empty.
        {
            Id = string.Empty;
            Email = string.Empty;
        }
        public Customer(string id, string email) // Constructeur qui initialise Id et Email avec les valeurs passées en paramètre. Pour créer un client directement avec Id et email.
        {
            Id = id;
            Email = email;
        }
    }
}
