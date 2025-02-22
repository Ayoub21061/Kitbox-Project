using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Models;
using Kitbox.Views;
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

        // Ajout de commandes pour ouvrir les pages suivantes
        public IRelayCommand SecondPageCommand { get; }

        public IRelayCommand ThirdPageCommand { get; }

        public CustomerViewModel()
        {
            Customer = new Customer();
            Height = 0;
            Width = 0;
            Depth = 0;

            // Initialisation de la commande Save
            SaveCommand = new RelayCommand(SaveCustomerData);
            // Ajout de la commande pour ouvrir la page suivante
            SecondPageCommand = new RelayCommand(SecondNextPage);
            ThirdPageCommand = new RelayCommand(ThirdNextPage); 
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

        private void SecondNextPage()
        {
            var SecondPage = new SecondPageView();
            SecondPage.Show();
        }

        private void ThirdNextPage()
        {
            var ThirdPage = new ThirdPageView();
            ThirdPage.Show();
        }

    }
}
