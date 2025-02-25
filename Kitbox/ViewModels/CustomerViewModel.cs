using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System;
using System.Collections.Generic;
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

        [ObservableProperty]
        private int lockers;

        [ObservableProperty]
        private string? confirm;

        // Commande de sauvegarde
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand SaveCommandWithLocker { get; }

        // Variable pour savoir si les données initiales ont été sauvegardées
        private bool _isInitialDataSaved;

        // Ajout de commandes pour ouvrir les pages suivantes
        public IRelayCommand SecondPageCommand { get; }

        public IRelayCommand ThirdPageCommand { get; }

        public IRelayCommand FourthPageCommand { get; }

        public CustomerViewModel()
        {
            Customer = new Customer();
            Height = 0;
            Width = 0;
            Depth = 0;
            Lockers = 0;
            _isInitialDataSaved = false;  // Initialiser à false car les données ne sont pas encore sauvegardées

            // Initialisation de la commande Save
            SaveCommand = new RelayCommand(SaveCustomerData);
            // Commande avec les lockers
            SaveCommandWithLocker = new RelayCommand(SaveCustomerDataWithLocker);
            // Ajout de la commande pour ouvrir la page suivante
            SecondPageCommand = new RelayCommand(SecondNextPage);
            ThirdPageCommand = new RelayCommand(ThirdNextPage);
            FourthPageCommand = new RelayCommand(FourthNextPage);
        }

        // Méthode pour sauvegarder les données du client (hauteur, largeur, profondeur)
        private void SaveCustomerData()
        {
            if (_isInitialDataSaved)
            {
                Confirm = "Les données de dimensions sont déjà sauvegardées et ne peuvent pas être modifiées.";
                return;  // Si les données sont déjà sauvegardées, ne rien faire
            }

            var customerData = new
            {
                Height = this.Height,
                Width = this.Width,
                Depth = this.Depth,
                Lockers = 0  // Définir locker à 0 initialement
            };

            string filePath = "customer_data.json"; // Le chemin du fichier de sauvegarde

            try
            {
                // Sauvegarder les données dans un fichier JSON
                string jsonString = JsonSerializer.Serialize(customerData);
                File.WriteAllText(filePath, jsonString);

                // Indiquer que les données ont été sauvegardées
                _isInitialDataSaved = true;
                Confirm = "Les données de dimensions ont été sauvegardées avec succès!";

            }
            catch (Exception ex)
            {
                // Gérer les erreurs lors de la sauvegarde
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }

        // Méthode pour sauvegarder les données du client avec les lockers
        // Méthode pour sauvegarder les données du client avec les lockers
        private void SaveCustomerDataWithLocker()
        {
            if (!_isInitialDataSaved)
            {
                Confirm = "Veuillez d'abord sauvegarder les dimensions avant de sauvegarder les lockers.";
                return; // Empêcher la sauvegarde des lockers tant que les dimensions ne sont pas sauvegardées
            }

            var customerData = new
            {
                Lockers = this.Lockers
            };

            string filePath = "customer_data.json"; // Le chemin du fichier de sauvegarde

            try
            {
                // Lire les données existantes depuis le fichier
                string jsonString = File.ReadAllText(filePath);

                // Vérification de si le fichier est vide ou si les données sont nulles
                var existingData = string.IsNullOrEmpty(jsonString)
                    ? new Dictionary<string, object>()
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                if (existingData == null)
                {
                    existingData = new Dictionary<string, object>(); // Créer un nouveau dictionnaire si le fichier est vide ou si la désérialisation échoue
                }

                // Mettre à jour la valeur du locker dans les données existantes
                existingData["Lockers"] = this.Lockers;

                // Sauvegarder à nouveau les données dans un fichier JSON
                string updatedJsonString = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, updatedJsonString);

                Confirm = "Les lockers ont été sauvegardés avec succès!";
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

        private void FourthNextPage()
        {
            var FourthPage = new FourthPageView();
            FourthPage.Show();
        }
    }
}
