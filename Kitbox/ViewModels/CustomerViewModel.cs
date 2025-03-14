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

        [ObservableProperty]
        private string? selectedIron;

        public List<string> Iron { get; } = new List<string> { "Blanc", "Noir", "Gris", "Rose" };

        // Commande de sauvegarde
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand SaveCommandWithLocker { get; }

        public IRelayCommand SaveCommandWithIron { get; }

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
            Iron = new List<string> { "Blanc", "Noir", "Gris", "Rose" };

            LockersList = new ObservableCollection<LockerViewModel>();
            LoadLockers();

            // Initialisation de la commande Save
            SaveCommand = new RelayCommand(SaveCustomerData);
            // Commande avec les lockers
            SaveCommandWithLocker = new RelayCommand(SaveCustomerDataWithLocker);
            // Sauvegarde des irons
            SaveCommandWithIron = new RelayCommand(SaveCustomerDataWithIron);
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

        private void SaveCustomerDataWithIron()
        {
            string filePath = "customer_data.json"; // Chemin du fichier JSON

            try
            {
                Dictionary<string, object> existingData = new Dictionary<string, object>();

                // Vérifier si le fichier existe et récupérer les données existantes
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();
                }

                // Vérifier si les dimensions existent dans le fichier JSON
                if (existingData.ContainsKey("Height") && existingData.ContainsKey("Width") && existingData.ContainsKey("Depth"))
                {
                    // Vérifier si une couleur a été sélectionnée
                    if (!string.IsNullOrEmpty(this.SelectedIron))
                    {
                        // Mettre à jour ou ajouter la couleur sélectionnée
                        existingData["Iron"] = this.SelectedIron;

                        // Sauvegarder les données mises à jour dans le fichier JSON
                        string updatedJsonString = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(filePath, updatedJsonString);

                        Confirm = $"Iron color '{this.SelectedIron}' successfully saved!";
                    }
                    else
                    {
                        Confirm = "Please select an iron color before saving!";
                    }
                }
                else
                {
                    Confirm = "Please save dimensions first!";
                }
            }
            catch (Exception ex)
            {
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

                    // Désérialisation des données existantes
                    var existingData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

                    if (existingData != null)
                    {
                        // Vérifier si la clé "Lockers" existe et est un nombre
                        if (existingData.ContainsKey("Lockers") && existingData["Lockers"].ValueKind == JsonValueKind.Number)
                        {
                            Lockers = existingData["Lockers"].GetInt32();  // Récupère la valeur de Lockers
                            LockersList.Clear();  // Vide la liste des lockers avant d'ajouter les nouveaux

                            for (int i = 0; i < Lockers; i++)
                            {
                                // Crée un nouvel objet LockerViewModel avec un index unique pour chaque locker
                                var locker = new LockerViewModel(i, this);
                                LockersList.Add(locker); // Ajoute à la liste
                            }
                        }
                        else
                        {
                            Console.WriteLine("La valeur de Lockers n'est pas un nombre ou elle est manquante.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Les données existantes sont vides ou mal formatées.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des lockers : {ex.Message}");
            }
        }

        public void DeleteLocker(int lockerIndex)
        {
            try
            {
                string filePath = "customer_data.json";
                if (!File.Exists(filePath)) return;

                string existingJson = File.ReadAllText(filePath);
                var existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson) ?? new Dictionary<string, object>();

                if (existingData.ContainsKey("LockersData"))
                {
                    var lockersList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializer.Serialize(existingData["LockersData"])) ?? new List<Dictionary<string, object>>();

                    // Supprimer le locker par index
                    if (lockerIndex >= 0 && lockerIndex < lockersList.Count)
                    {
                        lockersList.RemoveAt(lockerIndex);

                        // Mettre à jour les données existantes avec les nouvelles informations
                        existingData["LockersData"] = lockersList;
                        existingData["Lockers"] = lockersList.Count;

                        // Sauvegarder les données mises à jour
                        string newJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(filePath, newJson);

                        // Supprimer l'objet LockerViewModel correspondant
                        LockersList.RemoveAt(lockerIndex);

                        Console.WriteLine("Locker data successfully deleted!");
                    }
                    else
                    {
                        Console.WriteLine("No matching locker found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression : {ex.Message}");
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
