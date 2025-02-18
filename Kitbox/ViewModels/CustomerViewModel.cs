using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Models;
using System;
using System.IO;
using System.Text.Json;

namespace Kitbox.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        public Customer customer;

        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private int width;

        [ObservableProperty]
        private int depth;

        // Commande de sauvegarde
        public IRelayCommand SaveCommand { get; }

        public CustomerViewModel()
        {
            Customer = new Customer();
            Height = 0;
            Width = 0;
            Depth = 0;

            // Initialisation de la commande Save
            SaveCommand = new RelayCommand(SaveCustomerData);
        }

        // Méthode pour sauvegarder les données du client
        private void SaveCustomerData()
        {
            var customerData = new 
            {
                Height = this.Height,
                Width = this.Width,
                Depth = this.Depth
            };

            string filePath = "customer_data.json"; // Le chemin du fichier de sauvegarde

            try
            {
                // Sauvegarder les données dans un fichier JSON
                string jsonString = JsonSerializer.Serialize(customerData);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Gérer les erreurs lors de la sauvegarde
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }
    }
}
