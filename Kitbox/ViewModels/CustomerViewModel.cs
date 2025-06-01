using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kitbox.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        public Customer customer;

        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private int totalHeight;

        [ObservableProperty]
        private int width;

        [ObservableProperty]
        private int totalWidth;

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
        private string validationMessage;

        [ObservableProperty]
        public ObservableCollection<string>? iron;

        [ObservableProperty]
        public string? ironDisplay;

        [ObservableProperty]
        public ObservableCollection<string>? portes;

        [ObservableProperty]
        private string? porteSelected;

        [ObservableProperty]
        public decimal totalPrice;

        [ObservableProperty]
        private ObservableCollection<string> apercuLockerImages = new ObservableCollection<string>();

        public ObservableCollection<LockerData> LockersData { get; set; } = new ObservableCollection<LockerData>();




        public string ErrorMessage { get; set; } = "";

        // Commandes
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand SaveCommandWithLocker { get; }
        public IRelayCommand SaveCommandWithIron { get; }
        public IRelayCommand SecondPageCommand { get; }
        public IRelayCommand ThirdPageCommand { get; }
        public IRelayCommand FourthPageCommand { get; }
        public IRelayCommand FifthPageCommand { get; }
        public IRelayCommand SixthPageCommand { get; }
        public IRelayCommand FinalPageCommand { get; }
        public IRelayCommand VoirApercuCommand { get; }
        public IRelayCommand AppendCharacterCommand { get; }
        public IRelayCommand DeleteCharacterCommand { get; }
        public IRelayCommand SaveEmailCommand { get; }
        public IRelayCommand SaveDimensionsCommand { get; }
        public IRelayCommand ShowDetailsCommand { get; }
        public IRelayCommand BackToHomeCommand { get; }


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
            ValidationMessage = "";

            LockersList = new ObservableCollection<LockerViewModel>();
            StructureArmory = new ObservableCollection<ArmoryStructureRow>();

            // Chargement des données depuis le JSON (structure + lockers)
            LoadCombinedData();
            _ = LoadAnglesFromDatabaseAsync();
            LoadIronValue();
            LoadCustomerData();



            // Initialisation des commandes
            SaveCommand = new RelayCommand(SaveCustomerData);
            SaveCommandWithLocker = new RelayCommand(SaveCustomerDataWithLocker);
            SaveCommandWithIron = new RelayCommand(SaveCustomerDataWithIron);
            SecondPageCommand = new RelayCommand(SecondNextPage);
            ThirdPageCommand = new RelayCommand(ThirdNextPage);
            FourthPageCommand = new RelayCommand(FourthNextPage);
            FifthPageCommand = new RelayCommand(FifthNextPage);
            SixthPageCommand = new RelayCommand(SixthNextPage);
            FinalPageCommand = new RelayCommand(FinalPage);
            BackToHomeCommand = new RelayCommand(ReturnFirstPage);
            VoirApercuCommand = new RelayCommand(ExecuteVoirApercu);
            AppendCharacterCommand = new RelayCommand<string>(AppendCharacter);
            DeleteCharacterCommand = new RelayCommand(DeleteCharacter);
            SaveEmailCommand = new RelayCommand(SaveEmail);
            SaveDimensionsCommand = new RelayCommand(SaveDimension);
            ShowDetailsCommand = new RelayCommand(ShowPriceDetails);



        }

        private void ShowPriceDetails()
        {
            using var connection = new MySqlConnection("server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;");
            connection.Open();

            decimal totalPrice = 0;

            var articles = new List<string>();

            foreach (var locker in LockersData)
            {
                if (!string.IsNullOrEmpty(locker.PanelLeft))
                    articles.Add(locker.PanelLeft);
                if (!string.IsNullOrEmpty(locker.PanelRight))
                    articles.Add(locker.PanelRight);
                if (!string.IsNullOrEmpty(locker.PanelBottom))
                    articles.Add(locker.PanelBottom);
                if (!string.IsNullOrEmpty(locker.PanelUp))
                    articles.Add(locker.PanelUp);
                if (!string.IsNullOrEmpty(locker.TypeDePorte) && locker.TypeDePorte != "No Door")
                    articles.Add(locker.TypeDePorte);
            }

            if (!string.IsNullOrEmpty(SelectedIron))
                articles.Add(SelectedIron); // Ajouter l’élément principal Iron

            foreach (var article in articles.Distinct())
            {
                var getProductIdCmd = new MySqlCommand("SELECT product_id FROM Stock WHERE article_name = @article", connection);
                getProductIdCmd.Parameters.AddWithValue("@article", article);
                var productId = getProductIdCmd.ExecuteScalar()?.ToString();

                if (productId != null)
                {
                    var getPriceCmd = new MySqlCommand("SELECT article_price FROM Supplier_Product WHERE product_id = @id", connection);
                    getPriceCmd.Parameters.AddWithValue("@id", productId);
                    var price = getPriceCmd.ExecuteScalar();
                    if (price != null)
                        totalPrice += Convert.ToDecimal(price);
                }
            }

            // Affiche le total
            TotalPrice = totalPrice;
        }


        private void LoadCustomerData()
        {
            var jsonPath = "customer_data.json";
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                var doc = JsonSerializer.Deserialize<CustomerJsonData>(json);
                if (doc?.LockersData != null)
                {
                    LockersData = new ObservableCollection<LockerData>(doc.LockersData);
                }
            }
        }

        private class CustomerJsonData
        {
            public List<LockerData>? LockersData { get; set; }
        }

        private void LoadIronValue()
        {
            try
            {
                string jsonPath = "customer_data.json"; // adapte le chemin si besoin
                if (File.Exists(jsonPath))
                {
                    string jsonString = File.ReadAllText(jsonPath);
                    using JsonDocument doc = JsonDocument.Parse(jsonString);
                    JsonElement root = doc.RootElement;

                    if (root.TryGetProperty("Iron", out JsonElement ironElement))
                    {
                        IronDisplay = ironElement.GetString();
                    }
                    else
                    {
                        IronDisplay = "Iron key not found";
                    }
                }
                else
                {
                    IronDisplay = "File not found";
                }
            }
            catch (Exception ex)
            {
                IronDisplay = $"Error: {ex.Message}";
            }
        }



        public void SaveDimension()
        {
            string filePath = "customer_data.json";
            if (!File.Exists(filePath))
            {
                ValidationMessage = "Erreur : le fichier customer_data.json est introuvable.";
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);

                if (data == null)
                {
                    ValidationMessage = "Erreur : données invalides dans le fichier JSON.";
                    return;
                }

                int baseHeight = data.ContainsKey("Height") ? data["Height"].GetInt32() : 0;
                int baseWidth = data.ContainsKey("Width") ? data["Width"].GetInt32() : 0;

                double totalHeight = 0;
                double totalWidth = 0;

                if (data.ContainsKey("LockersData") && data["LockersData"].ValueKind == JsonValueKind.Array)
                {
                    var lockersData = data["LockersData"].EnumerateArray();
                    foreach (var locker in lockersData)
                    {
                        int longueur = locker.TryGetProperty("LongueurVertical", out var l) ? l.GetInt32() : 0;
                        int largeur = locker.TryGetProperty("LargeurHorizontal", out var w) ? w.GetInt32() : 0;

                        totalHeight += longueur;
                        totalWidth += largeur;
                    }
                }

                if (totalHeight > baseHeight && totalWidth > baseWidth)
                {
                    ValidationMessage = "Erreur : la longueur ET la largeur totales dépassent les dimensions initiales.";
                }
                else if (totalHeight > baseHeight)
                {
                    ValidationMessage = "Erreur : la longueur totale dépasse la longueur initiale.";
                }
                else if (totalWidth > baseWidth)
                {
                    ValidationMessage = "Erreur : la largeur totale dépasse la largeur initiale.";
                }
                else
                {
                    ValidationMessage = "Dimensions valides et sauvegardées avec succès !";
                }

                // Mise à jour des propriétés bindées
                TotalHeight = (int)totalHeight;
                TotalWidth = (int)totalWidth;
            }
            catch (Exception ex)
            {
                ValidationMessage = $"Erreur lors de la lecture du fichier : {ex.Message}";
            }
        }

        public async Task LoadAnglesFromDatabaseAsync()
        {
            try
            {
                var angleList = new ObservableCollection<string>();
                string connectionString = "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                string query = "SELECT article_name FROM Stock WHERE article_name LIKE '%Angle%'";

                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    angleList.Add(reader.GetString("article_name"));
                }

                Iron = angleList;

                if (Iron.Count > 0)
                    SelectedIron = Iron[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur chargement des irons : {ex.Message}");
            }
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
                    TotalHeight = CalculateSumLengthVerticalFromJson();

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
                                LongueurHorizontal = lockerData.TryGetProperty("LongueurHorizontal", out var longueurHorizontal) ? longueurHorizontal.GetInt32() : 0,
                                LargeurHorizontal = lockerData.TryGetProperty("LargeurHorizontal", out var largeurHorizontal) ? largeurHorizontal.GetInt32() : 0,
                                LongueurVertical = lockerData.TryGetProperty("LongueurVertical", out var longueurVertical) ? longueurVertical.GetInt32() : 0,
                                LargeurVertical = lockerData.TryGetProperty("LargeurVertical", out var largeurVertical) ? largeurVertical.GetInt32() : 0,
                                HasPorte = lockerData.TryGetProperty("HasPorte", out var hasPorte) ? hasPorte.GetBoolean() : false,
                                PorteSelected = lockerData.TryGetProperty("TypeDePorte", out var porteSelected) ? porteSelected.GetString() : "",
                                PanelLeft = lockerData.TryGetProperty("PanelLeft", out var panelLeft) ? panelLeft.GetString() : "",
                                PanelRight = lockerData.TryGetProperty("PanelRight", out var panelRight) ? panelRight.GetString() : "",
                                PanelBottom = lockerData.TryGetProperty("PanelBottom", out var panelBottom) ? panelBottom.GetString() : "",
                                PanelUp = lockerData.TryGetProperty("PanelUp", out var panelUp) ? panelUp.GetString() : "",
                                CrossbarLeft = lockerData.TryGetProperty("CrossbarLeft", out var crossbarLeft) ? crossbarLeft.GetString() : "",
                                CrossbarRight = lockerData.TryGetProperty("CrossbarRight", out var crossbarRight) ? crossbarRight.GetString() : "",
                                CrossbarBack = lockerData.TryGetProperty("CrossbarBack", out var crossbarBack) ? crossbarBack.GetString() : "",
                                BattenSelected = lockerData.TryGetProperty("BattenSelected", out var battenSelected) ? battenSelected.GetString() : "",

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

        public int CalculateSumLengthVerticalFromJson()
        {
            string filePath = "customer_data.json";
            if (!File.Exists(filePath))
                return 0;

            string jsonContent = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);
            if (data == null || !data.ContainsKey("LockersData"))
                return 0;

            int sumLengthVertical = 0;
            var lockersArray = data["LockersData"].EnumerateArray();
            foreach (var locker in lockersArray)
            {
                if (locker.TryGetProperty("LongueurVertical", out JsonElement longueurVertProp))
                {
                    sumLengthVertical += longueurVertProp.GetInt32();
                }
            }
            return sumLengthVertical;
        }

        public int CalculateSumWidthHorizontalFromJson()
        {
            string filePath = "customer_data.json";
            if (!File.Exists(filePath))
                return 0;

            string jsonContent = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);
            if (data == null || !data.ContainsKey("LockersData"))
                return 0;

            int sumWidthHorizontal = 0;
            var lockersArray = data["LockersData"].EnumerateArray();
            foreach (var locker in lockersArray)
            {
                if (locker.TryGetProperty("LargeurHorizontal", out JsonElement largeurHorProp))
                {
                    sumWidthHorizontal += largeurHorProp.GetInt32();
                }
            }
            return sumWidthHorizontal;
        }


        public bool CanGoNextPage()
        {
            int totalLength = CalculateSumLengthVerticalFromJson(); // longueur totale des lockers
            int totalWidth = CalculateSumWidthHorizontalFromJson();   // largeur totale des lockers (à implémenter)

            bool lengthExceeded = totalLength > Height;
            bool widthExceeded = totalWidth > Width;

            if (lengthExceeded && widthExceeded)
            {
                ErrorMessage = $"Votre composition dépasse la hauteur maximale ({Height} cm) ET la largeur maximale ({Width} cm). " +
                               $"Longueur totale = {totalLength} cm, Largeur totale = {totalWidth} cm.";
                return false;
            }
            else if (lengthExceeded)
            {
                ErrorMessage = $"Votre composition dépasse la hauteur maximale ({Height} cm). Longueur totale = {totalLength} cm.";
                return false;
            }
            else if (widthExceeded)
            {
                ErrorMessage = $"Votre composition dépasse la largeur maximale ({Width} cm). Largeur totale = {totalWidth} cm.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
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
            if (CanGoNextPage())
            {
                var FourthPage = new FourthPageView();
                FourthPage.Show();
            }
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
        private void FinalPage()
        {
            var FinalPage = new FinalPageView();
            FinalPage.Show();
        }

        private void ReturnFirstPage()
        {
            var mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}
