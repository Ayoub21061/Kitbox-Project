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
        private string? selectedCouleur;

        [ObservableProperty]
        public string? panelLeft;

        [ObservableProperty]
        public string? panelRight;

        [ObservableProperty]
        public string? panelBottom;

        [ObservableProperty]
        public string? panelUp;

        [ObservableProperty]
        private int longueurHorizontal;

        [ObservableProperty]
        private int largeurHorizontal;

        [ObservableProperty]
        private int longueurVertical;

        [ObservableProperty]
        private int largeurVertical;

        [ObservableProperty]
        private bool isDimensionsReadOnly = false;

        [ObservableProperty]
        private ObservableCollection<string>? portes;

        [ObservableProperty]
        private bool hasPorte;

        [ObservableProperty]
        private string? porteSelected;

        public IRelayCommand SaveLockersViewCommand { get; }
        public IRelayCommand DeleteLockerCommand { get; }

        [ObservableProperty]
        private ObservableCollection<string> panels = new(); // Panels left/right

        [ObservableProperty]
        private ObservableCollection<string> horizontalPanels = new(); // Panels up/bottom

        // [ObservableProperty]
        // public int panelBottomLength;

        // [ObservableProperty]
        // public int panelBottomWidth;

        // [ObservableProperty]
        // public int panelUpLength;

        // [ObservableProperty]
        // public int panelUpWidth;

        public int LockerIndex { get; set; }

        public string LockerLabel => $"Locker {LockerIndex + 1}";

        public LockerViewModel(int index, CustomerViewModel customerViewModel)
        {
            _customerViewModel = customerViewModel;
            SaveLockersViewCommand = new RelayCommand(() => SaveLockerData(index));
            DeleteLockerCommand = new RelayCommand(DeleteLockerData);
            LockerIndex = index;
            _ = LoadPanelsFromDatabaseAsync();
            _ = LoadPortesFromDatabaseAsync();
            LoadLockerData(index);

        }

        public void LoadLockerData(int index)
        {
            try
            {
                string filePath = "customer_data.json";

                if (!File.Exists(filePath))
                    return;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (data == null || !data.ContainsKey("LockersData"))
                    return;

                var lockersJson = JsonSerializer.Serialize(data["LockersData"]);
                var lockersList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(lockersJson);

                if (lockersList == null || index >= lockersList.Count)
                    return;

                var lockerData = lockersList[index];

                PanelLeft = lockerData.GetValueOrDefault("PanelLeft")?.ToString();
                PanelRight = lockerData.GetValueOrDefault("PanelRight")?.ToString();
                PanelBottom = lockerData.GetValueOrDefault("PanelBottom")?.ToString();
                PanelUp = lockerData.GetValueOrDefault("PanelUp")?.ToString();

                LongueurHorizontal = lockerData.TryGetValue("LongueurHorizontal", out var lH) ? Convert.ToInt32(lH) : 0;
                LargeurHorizontal = lockerData.TryGetValue("LargeurHorizontal", out var Lh) ? Convert.ToInt32(Lh) : 0;
                LongueurVertical = lockerData.TryGetValue("LongueurVertical", out var lV) ? Convert.ToInt32(lV) : 0;
                LargeurVertical = lockerData.TryGetValue("LargeurVertical", out var Lv) ? Convert.ToInt32(Lv) : 0;

                HasPorte = lockerData.TryGetValue("HasPorte", out var hp) && Convert.ToBoolean(hp);
                PorteSelected = lockerData.GetValueOrDefault("TypeDePorte")?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du casier : {ex.Message}");
            }
        }


        private void SaveLockerData(int index)
        {
            try
            {
                string filePath = "customer_data.json";
                Dictionary<string, object> existingData = new Dictionary<string, object>();

                if (File.Exists(filePath))
                {
                    string existingJson = File.ReadAllText(filePath);
                    existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson) ?? new Dictionary<string, object>();
                }

                List<Dictionary<string, object>> lockersList = new();

                // Charger la liste actuelle
                if (existingData.ContainsKey("LockersData"))
                {
                    var tempJson = JsonSerializer.Serialize(existingData["LockersData"]);
                    lockersList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(tempJson) ?? new List<Dictionary<string, object>>();
                }

                var lockerData = new Dictionary<string, object>
        {
            { "PanelLeft", PanelLeft ?? "" },
            { "PanelRight", PanelRight ?? "" },
            { "PanelBottom", PanelBottom ?? "" },
            { "PanelUp", PanelUp ?? "" },
            { "LongueurHorizontal", LongueurHorizontal },
            { "LargeurHorizontal", LargeurHorizontal },
            { "LongueurVertical", LongueurVertical },
            { "LargeurVertical", LargeurVertical },
            { "HasPorte", HasPorte },
            { "TypeDePorte", PorteSelected ?? "" }
        };

                // Mise à jour du casier à l'index correspondant
                if (index < lockersList.Count)
                {
                    lockersList[index] = lockerData;
                }
                else
                {
                    lockersList.Add(lockerData);
                }

                // Mise à jour des autres données
                existingData["LockersData"] = lockersList;
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
                var sideList = new ObservableCollection<string>();
                var horizontalList = new ObservableCollection<string>();

                string connectionString = "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "SELECT article_name FROM Stock WHERE article_name LIKE '%Panel%'";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string name = reader.GetString("article_name").ToLower();

                    if (name.Contains("panel") && (name.Contains("left") || name.Contains("right")))
                        sideList.Add(reader.GetString("article_name"));

                    if (name.Contains("panel") && name.Contains("horizontal"))
                        horizontalList.Add(reader.GetString("article_name"));
                }

                Panels = sideList;
                HorizontalPanels = horizontalList;

                if (Panels.Count > 0)
                {
                    PanelLeft = Panels[0];
                    PanelRight = Panels[0];
                }

                if (HorizontalPanels.Count > 0)
                {
                    PanelBottom = HorizontalPanels[0];
                    PanelUp = HorizontalPanels[0];
                }
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

        partial void OnPanelLeftChanged(string? oldValue, string? newValue) => UpdateDimensionsFromSelectedPanel(newValue, isVertical: true);
        partial void OnPanelRightChanged(string? oldValue, string? newValue) => UpdateDimensionsFromSelectedPanel(newValue, isVertical: true);
        partial void OnPanelBottomChanged(string? oldValue, string? newValue) => UpdateDimensionsFromSelectedPanel(newValue, isVertical: false);
        partial void OnPanelUpChanged(string? oldValue, string? newValue) => UpdateDimensionsFromSelectedPanel(newValue, isVertical: false);

        private void UpdateDimensionsFromSelectedPanel(string? panelName, bool isVertical)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                IsDimensionsReadOnly = false;
                return;
            }

            var regex = new System.Text.RegularExpressions.Regex(@"(\d+)\((h|p|l|H|P|L)\)\s*x\s*(\d+)\((h|p|l|H|P|L)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var match = regex.Match(panelName);

            if (match.Success)
            {
                var firstValue = int.Parse(match.Groups[1].Value);
                var firstUnit = match.Groups[2].Value.ToLower();
                var secondValue = int.Parse(match.Groups[3].Value);
                var secondUnit = match.Groups[4].Value.ToLower();

                int largeurParsed = 0;
                int longueurParsed = 0;

                if (firstUnit == "p")
                    largeurParsed = firstValue;
                else if (secondUnit == "p")
                    largeurParsed = secondValue;

                if (firstUnit == "l" || firstUnit == "h")
                    longueurParsed = firstValue;
                else if (secondUnit == "l" || secondUnit == "h")
                    longueurParsed = secondValue;

                if (isVertical)
                {
                    LargeurVertical = largeurParsed;
                    LongueurVertical = longueurParsed;
                }
                else
                {
                    LargeurHorizontal = largeurParsed;
                    LongueurHorizontal = longueurParsed;
                }

                IsDimensionsReadOnly = true;
                return;
            }

            IsDimensionsReadOnly = false;
        }
    }
}
