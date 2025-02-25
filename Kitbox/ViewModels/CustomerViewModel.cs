using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // Ajout de commandes pour ouvrir les pages suivantes
        public IRelayCommand SecondPageCommand { get; }

        public IRelayCommand ThirdPageCommand { get; }

        public IRelayCommand FourthPageCommand { get; }

        public ObservableCollection<LockerViewModel> LockersList { get; } = new ObservableCollection<LockerViewModel>();


        public CustomerViewModel()
        {
            Customer = new Customer();
            Height = 0;
            Width = 0;
            Depth = 0;
            Lockers = 0;

            LockersList = new ObservableCollection<LockerViewModel>();
            LoadLockers();

            // Initialisation de la commande Save
            SaveCommand = new RelayCommand(SaveCustomerData);
            // Commande avec les lockers
            SaveCommandWithLocker = new RelayCommand(SaveCustomerDataWithLocker);
            // Ajout de la commande pour ouvrir la page suivante
            SecondPageCommand = new RelayCommand(SecondNextPage);
            ThirdPageCommand = new RelayCommand(ThirdNextPage); 
            FourthPageCommand = new RelayCommand(FourthNextPage);
        }

        // Méthode pour sauvegarder les données du client (Height, Width, Depth)
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
                string jsonString = JsonSerializer.Serialize(customerData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);

                // Afficher un message de confirmation
                Confirm = "Data successfully saved!";
            }
            catch (Exception ex)
            {
                // Gérer les erreurs lors de la sauvegarde
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }

        // Méthode pour sauvegarder les données du client avec Lockers
        private void SaveCustomerDataWithLocker()
        {
            string filePath = "customer_data.json"; // Le chemin du fichier de sauvegarde
            try
            {
                // Lire les données précédemment sauvegardées pour Height, Width, Depth
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    var existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                    // Vérifiez si les données de dimensions existent
                    if (existingData != null && existingData.ContainsKey("Height") && existingData.ContainsKey("Width") && existingData.ContainsKey("Depth"))
                    {
                        // Mettre à jour les données avec la valeur de Lockers
                        existingData["Lockers"] = this.Lockers;

                        // Sauvegarder les données mises à jour
                        string updatedJsonString = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(filePath, updatedJsonString);

                        Confirm = "Data with Lockers successfully saved!";
                    }
                    else
                    {
                        Confirm = "Please save dimensions first!";
                    }
                }
                else
                {
                    Confirm = "No previous data found. Please save dimensions first!";
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs lors de la sauvegarde
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }

        private void LoadLockers()
        {
            string filePath = "customer_data.json";

            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    var existingData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

                    if (existingData != null && existingData.ContainsKey("Lockers"))
                    {
                        // Assure-toi de vérifier et de convertir correctement le JsonElement en int
                        if (existingData["Lockers"].ValueKind == JsonValueKind.Number)
                        {
                            Lockers = existingData["Lockers"].GetInt32();

                            // Créer des objets LockerViewModel en fonction de la valeur de Lockers
                            LockersList.Clear();  // Vide la liste précédente si nécessaire

                            for (int i = 0; i < Lockers; i++)
                            {
                                // Crée un nouvel objet LockerViewModel pour chaque locker
                                var locker = new LockerViewModel();
                                LockersList.Add(locker);
                            }
                        }
                        else
                        {
                            // Gérer le cas où la valeur de "Lockers" n'est pas un nombre
                            Console.WriteLine("La valeur de Lockers n'est pas un nombre.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des lockers : {ex.Message}");
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
