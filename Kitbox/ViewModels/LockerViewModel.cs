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
    public partial class LockerViewModel : ObservableObject
    {
        private readonly CustomerViewModel _customerViewModel;

        [ObservableProperty]
        private List<string> couleur; // Liste des couleurs

        [ObservableProperty]
        private string? selectedCouleur; // La couleur sélectionnée

        [ObservableProperty]
        private int longueur;

        [ObservableProperty]
        private bool hasPorte;

        [ObservableProperty]
        private List<string> couleurPorte;

        [ObservableProperty]
        private string? couleurPorteSelected;

        [ObservableProperty]
        private string? matérielPorte;

        public List<string> MatérielOptions { get; } = new() { "Bois", "Métal", "Plastique" };

        public IRelayCommand SaveLockersViewCommand { get; }
        public IRelayCommand DeleteLockerCommand { get; }

        public int LockerIndex { get; set; }  // Indice unique pour chaque locker

        // Propriété pour afficher l'étiquette du locker
        public string LockerLabel => $"Locker {LockerIndex + 1}";

        public LockerViewModel(int index, CustomerViewModel customerViewModel)
        {
            _customerViewModel = customerViewModel;
            SaveLockersViewCommand = new RelayCommand(SaveLockerData);
            DeleteLockerCommand = new RelayCommand(DeleteLockerData);
            Couleur = new List<string> { "Blanc", "Noir", "Gris", "Rose" };
            CouleurPorte = new List<string> { "Blanc", "Noir", "Gris" };
            LockerIndex = index;
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
                    { "Couleur", SelectedCouleur ?? "" },
                    { "Longueur", Longueur },
                    { "HasPorte", HasPorte },
                    { "CouleurPorte", CouleurPorteSelected ?? "" },
                    { "MaterielPorte", MatérielPorte ?? "" }
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
    }
}
