using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private ObservableCollection<string> apercuLockerImages = new ObservableCollection<string>();

        public string ErrorMessage { get; set; } = "";


        public List<string> Iron { get; } = new List<string> { "Blanc", "Noir", "Gris", "Rose" };

        // Commandes
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand SaveCommandWithLocker { get; }
        public IRelayCommand SaveCommandWithIron { get; }
        public IRelayCommand SecondPageCommand { get; }
        public IRelayCommand ThirdPageCommand { get; }
        public IRelayCommand FourthPageCommand { get; }
        public IRelayCommand FifthPageCommand { get; }
        public IRelayCommand SixthPageCommand { get; }
        public IRelayCommand VoirApercuCommand { get; }
        public IRelayCommand AppendCharacterCommand { get; }
        public IRelayCommand DeleteCharacterCommand { get; }
        public IRelayCommand SaveEmailCommand { get; }

        // Collection pour les données des lockers
        public ObservableCollection<LockerViewModel> LockersList { get; } = new ObservableCollection<LockerViewModel>();
        // Collection pour le tableau "Structure de l'armoire"
        public ObservableCollection<ArmoryStructureRow> StructureArmory { get; } = new ObservableCollection<ArmoryStructureRow>();

        public CustomerViewModel()
        {
            Customer = new Customer();
            Height = 0;
            Width = 0;
            Depth = 0;
            Lockers = 0;
            SelectedIron = "";
            Email = "";

            LockersList = new ObservableCollection<LockerViewModel>();
            StructureArmory = new ObservableCollection<ArmoryStructureRow>();

            // Chargement des données depuis le JSON (structure + lockers)
            LoadCombinedData();

            // Initialisation des commandes
            SaveCommand = new RelayCommand(SaveCustomerData);
            SaveCommandWithLocker = new RelayCommand(SaveCustomerDataWithLocker);
            SaveCommandWithIron = new RelayCommand(SaveCustomerDataWithIron);
            SecondPageCommand = new RelayCommand(SecondNextPage);
            ThirdPageCommand = new RelayCommand(ThirdNextPage);
            FourthPageCommand = new RelayCommand(FourthNextPage);
            FifthPageCommand = new RelayCommand(FifthNextPage);
            SixthPageCommand = new RelayCommand(SixthNextPage);
            VoirApercuCommand = new RelayCommand(ExecuteVoirApercu);
            AppendCharacterCommand = new RelayCommand<string>(AppendCharacter);
            DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
            SaveEmailCommand = new RelayCommand(SaveEmail);
        }

        public bool IsEmailValid()
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(Email, emailPattern);
        }

        public void ValidateEmail()
        {
            if (!IsEmailValid())
            {
                ErrorMessage = "Adresse mail invalide. Veuillez réessayer.";
            }
            else
            {
                ErrorMessage = string.Empty;
            }
        }
        private void AppendCharacter(string? character)
        {
            Email += character;
        }

        private void DeleteCharacter()
        {
            if (!string.IsNullOrEmpty(Email))
            {
                Email = Email.Substring(0, Email.Length - 1);
            }
        }

        public void SaveEmail()
        {
            // Valider l'email avant de le sauvegarder
            ValidateEmail();

            if (string.IsNullOrEmpty(ErrorMessage)) // Si pas d'erreur, on sauvegarde
            {
                string filepath = "customer_data.json";
                Dictionary<string, object> existingData = new Dictionary<string, object>();

                // Chargement des données existantes dans le fichier JSON
                if (File.Exists(filepath))
                {
                    string jsonString = File.ReadAllText(filepath);
                    existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();
                }

                // Ajout ou mise à jour de l'email dans le dictionnaire
                existingData["Email"] = Email;

                // Sérialisation et sauvegarde des données mises à jour dans le fichier JSON
                string updatedJsonString = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filepath, updatedJsonString);

                Confirm = "Email successfully saved!";
            }
            else
            {
                // Si l'email est invalide, on affiche le message d'erreur
                Confirm = ErrorMessage;
            }
        }




        // Méthode unique de chargement combiné
        public void LoadCombinedData()
        {
            string filePath = "customer_data.json";
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);
                if (data != null)
                {
                    // Chargement des données de base
                    Height = data.ContainsKey("Height") ? data["Height"].GetInt32() : 0;
                    Width = data.ContainsKey("Width") ? data["Width"].GetInt32() : 0;
                    Depth = data.ContainsKey("Depth") ? data["Depth"].GetInt32() : 0;
                    Lockers = data.ContainsKey("Lockers") ? data["Lockers"].GetInt32() : 0;
                    SelectedIron = data.ContainsKey("Iron") ? data["Iron"].GetString() : "";

                    // Mise à jour du tableau "Structure de l'armoire" avec la classe ArmoryStructureRow
                    StructureArmory.Clear();
                    StructureArmory.Add(new ArmoryStructureRow("Height", Height.ToString()));
                    StructureArmory.Add(new ArmoryStructureRow("Width", Width.ToString()));
                    StructureArmory.Add(new ArmoryStructureRow("Depth", Depth.ToString()));
                    StructureArmory.Add(new ArmoryStructureRow("Lockers", Lockers.ToString()));
                    StructureArmory.Add(new ArmoryStructureRow("Iron", SelectedIron ?? ""));

                    // Chargement des lockers via LockersData
                    LockersList.Clear();
                    int index = 0;
                    if (data.ContainsKey("LockersData") && data["LockersData"].ValueKind == JsonValueKind.Array)
                    {
                        var lockersData = data["LockersData"].EnumerateArray();
                        foreach (var lockerData in lockersData)
                        {
                            var locker = new LockerViewModel(index, this)
                            {
                                SelectedCouleur = lockerData.TryGetProperty("Couleur", out var couleur) ? couleur.GetString() : "",
                                Longueur = lockerData.TryGetProperty("Longueur", out var longueur) ? longueur.GetInt32() : 0,
                                HasPorte = lockerData.TryGetProperty("HasPorte", out var hasPorte) ? hasPorte.GetBoolean() : false,
                                CouleurPorteSelected = lockerData.TryGetProperty("CouleurPorte", out var couleurPorte) ? couleurPorte.GetString() : "",
                                MatérielPorte = lockerData.TryGetProperty("MaterielPorte", out var materielPorte) ? materielPorte.GetString() : ""
                            };
                            LockersList.Add(locker);
                            index++;
                        }
                    }
                    // Compléter la liste si le nombre de lockers est inférieur à la valeur indiquée
                    while (LockersList.Count < Lockers)
                    {
                        var locker = new LockerViewModel(index, this);
                        LockersList.Add(locker);
                        index++;
                    }
                }
            }
        }

        // Méthode pour sauvegarder les dimensions uniquement
        private void SaveCustomerData()
        {
            var customerData = new
            {
                Height = this.Height,
                Width = this.Width,
                Depth = this.Depth
            };

            string filePath = "customer_data.json";
            try
            {
                string jsonString = JsonSerializer.Serialize(customerData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);
                Confirm = "Data successfully saved!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }

        // Méthode pour sauvegarder avec les lockers
        private void SaveCustomerDataWithLocker()
        {
            string filePath = "customer_data.json";
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    var existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                    if (existingData != null && existingData.ContainsKey("Height") && existingData.ContainsKey("Width") && existingData.ContainsKey("Depth"))
                    {
                        existingData["Lockers"] = this.Lockers;
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
                Console.WriteLine($"Erreur de sauvegarde : {ex.Message}");
            }
        }

        // Méthode pour sauvegarder la couleur Iron
        private void SaveCustomerDataWithIron()
        {
            string filePath = "customer_data.json";
            try
            {
                Dictionary<string, object> existingData = new Dictionary<string, object>();
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();
                }
                if (existingData.ContainsKey("Height") && existingData.ContainsKey("Width") && existingData.ContainsKey("Depth"))
                {
                    if (!string.IsNullOrEmpty(this.SelectedIron))
                    {
                        existingData["Iron"] = this.SelectedIron;
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
                    if (lockerIndex >= 0 && lockerIndex < lockersList.Count)
                    {
                        lockersList.RemoveAt(lockerIndex);
                        existingData["LockersData"] = lockersList;
                        existingData["Lockers"] = lockersList.Count;
                        string newJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(filePath, newJson);
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

        private void ExecuteVoirApercu()
        {
            // Utilise directement la propriété Lockers du ViewModel
            int lockersCount = this.Lockers;

            // Réinitialiser la collection avant de l'alimenter
            ApercuLockerImages.Clear();
            for (int i = 0; i < lockersCount; i++)
            {
                // Ajoute le chemin de l'image pour chaque locker
                ApercuLockerImages.Add("avares://Kitbox/Assets/Locker.png");
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

        private void FifthNextPage()
        {
            var FifthPage = new FifthPageView();
            FifthPage.Show();
        }

        private void SixthNextPage()
        {
            var SixthPage = new SixthPageView();
            SixthPage.Show();
        }
    }
}
