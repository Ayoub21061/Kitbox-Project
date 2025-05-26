using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Kitbox.ViewModels
{
    public partial class LockerViewModel : ObservableObject
    {
        private readonly CustomerViewModel _customerViewModel;


        [ObservableProperty]
        private string? selectedCouleur; // La couleur sélectionnée

        [ObservableProperty]
        private string? selectedPanel;

        [ObservableProperty]
        private int longueur;

        [ObservableProperty]
        private int largeur;

        [ObservableProperty]
        private bool isDimensionsReadOnly = false;

        [ObservableProperty]
        private ObservableCollection<string> portes;

        [ObservableProperty]
        private bool hasPorte;

        [ObservableProperty]
        private string? porteSelected;

        public IRelayCommand SaveLockersViewCommand { get; }
        public IRelayCommand DeleteLockerCommand { get; }

        [ObservableProperty]
        private ObservableCollection<string> panels = new ObservableCollection<string>();

        public int LockerIndex { get; set; }  // Indice unique pour chaque locker

        // Propriété pour afficher l'étiquette du locker
        public string LockerLabel => $"Locker {LockerIndex + 1}";

        public LockerViewModel(int index, CustomerViewModel customerViewModel)
        {
            _customerViewModel = customerViewModel;
            SaveLockersViewCommand = new RelayCommand(SaveLockerData);
            DeleteLockerCommand = new RelayCommand(DeleteLockerData);
            LockerIndex = index;
            _ = LoadPanelsFromDatabaseAsync();
            _ = LoadPortesFromDatabaseAsync();
        }

        private void SaveLockerData()
        {
            try
            {
                string filePath = "customer_data.json";
                List<Dictionary<string, object>> lockersList = new();

                Dictionary<string, object> existingData = new Dictionary<string, object>();
                if (File.Exists(filePath))
                {
                    string existingJson = File.ReadAllText(filePath);
                    existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson) ?? new Dictionary<string, object>();
                }

                var height = existingData.ContainsKey("Height") && existingData["Height"] is JsonElement heightElement ? heightElement.GetInt32() : 0;
                var width = existingData.ContainsKey("Width") && existingData["Width"] is JsonElement widthElement ? widthElement.GetInt32() : 0;
                var depth = existingData.ContainsKey("Depth") && existingData["Depth"] is JsonElement depthElement ? depthElement.GetInt32() : 0;

                var lockerData = new Dictionary<string, object>
                {
                    { "Panel", SelectedPanel ?? "" },
                    { "Longueur", Longueur },
                    { "Largeur", Largeur},
                    { "HasPorte", HasPorte },
                    { "TypeDePorte", PorteSelected ?? "" },
    
                };

                if (existingData.ContainsKey("LockersData"))
                {
                    lockersList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializer.Serialize(existingData["LockersData"])) ?? new List<Dictionary<string, object>>();
                }
                lockersList.Add(lockerData);

                existingData["LockersData"] = lockersList;
                existingData["Height"] = height;
                existingData["Width"] = width;
                existingData["Depth"] = depth;
                existingData["Lockers"] = lockersList.Count;

                string newJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, newJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement : {ex.Message}");
            }
        }

        private void DeleteLockerData()
        {
            _customerViewModel.DeleteLocker(LockerIndex);
        }
        

        public async Task LoadPanelsFromDatabaseAsync()
        {
            try
            {
                var panelsList = new ObservableCollection<string>();
                string connectionString = "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "SELECT article_name FROM Stock WHERE article_name LIKE '%Panel%'";
                ;

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    panelsList.Add(reader.GetString("article_name"));
                }

                Panels = panelsList;

                if (Panels.Count > 0)
                    SelectedPanel = Panels[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur chargement panels : {ex.Message}");
            }
        }

        public async Task LoadPortesFromDatabaseAsync()
        {
            try
            {
                var portesList = new ObservableCollection<string>();
                string connectionString = "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "SELECT article_name FROM Stock WHERE article_name LIKE '%Door%'";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    portesList.Add(reader.GetString("article_name"));
                }

                Portes = portesList;

                if (Portes.Count > 0)
                    PorteSelected = Portes[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur chargement des portes : {ex.Message}");
            }
        }

        partial void OnSelectedPanelChanged(string? oldValue, string? newValue)
        {
            UpdateDimensionsFromSelectedPanel();
        }

        private void UpdateDimensionsFromSelectedPanel()
        {
            if (string.IsNullOrEmpty(SelectedPanel))
            {
                IsDimensionsReadOnly = false;
                return;
            }

            // Regex pour trouver deux couples nombre+lettre entre parenthèses, séparés par 'x', peu importe l'ordre
            var regex = new System.Text.RegularExpressions.Regex(@"(\d+)\((h|p|l|H|P|L)\)\s*x\s*(\d+)\((h|p|l|H|P|L)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = regex.Match(SelectedPanel);

            if (match.Success)
            {
                // Récupérer les groupes
                var firstValue = int.Parse(match.Groups[1].Value);
                var firstUnit = match.Groups[2].Value.ToLower();
                var secondValue = int.Parse(match.Groups[3].Value);
                var secondUnit = match.Groups[4].Value.ToLower();

                // Initialiser variables dimension
                int largeurParsed = 0;
                int longueurParsed = 0;

                // Assigner selon unité
                // h = hauteur, p = profondeur, l = longueur (exemple)
                // On suppose ici que largeur correspond à profondeur (p), longueur correspond à longueur (l) ou hauteur (h) selon contexte.

                // Exemple, pour ton besoin :
                // Largeur = valeur associée à (p)
                // Longueur = valeur associée à (l) ou (h)

                // Assigner largeur (p)
                if (firstUnit == "p")
                    largeurParsed = firstValue;
                else if (secondUnit == "p")
                    largeurParsed = secondValue;

                // Assigner longueur (l ou h)
                if (firstUnit == "l" || firstUnit == "h")
                    longueurParsed = firstValue;
                else if (secondUnit == "l" || secondUnit == "h")
                    longueurParsed = secondValue;

                // Si on n’a pas trouvé une dimension, on met 0 par défaut
                Largeur = largeurParsed;
                Longueur = longueurParsed;
                IsDimensionsReadOnly = true;
                return;
            }

            // Sinon
            IsDimensionsReadOnly = false;
        }
    }
}
