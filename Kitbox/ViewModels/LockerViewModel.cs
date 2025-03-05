using System;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace Kitbox.ViewModels;

public partial class LockerViewModel : ObservableObject
{
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

    public ICommand SaveLockersViewCommand { get; }
    public ICommand DeleteLockerCommand { get; }

    public int LockerIndex { get; set; }  // Indice unique pour chaque locker


    public LockerViewModel(int index)
    {
        SaveLockersViewCommand = new RelayCommand(SaveLockerData);
        DeleteLockerCommand = new RelayCommand<int>(DeleteLockerData);
        Couleur = new List<string> { "Blanc", "Noir", "Gris", "Rose" }; // Liste des couleurs disponibles
        CouleurPorte = new List<string> { "Blanc", "Noir", "Gris" };  // Liste des couleurs de porte
        LockerIndex = index;  // Assigner l'index du locker
    }

    private void SaveLockerData()
    {
        try
        {
            string filePath = "customer_data.json";
            List<Dictionary<string, object>> lockersList = new();

            // Lire les données existantes si elles existent
            Dictionary<string, object> existingData = new Dictionary<string, object>();

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                existingData = JsonSerializer.Deserialize<Dictionary<string, object>>(existingJson) ?? new Dictionary<string, object>();
            }

            // Récupérer ou initialiser les anciennes valeurs en les extrayant correctement du JsonElement
            var height = existingData.ContainsKey("Height") && existingData["Height"] is JsonElement heightElement ? heightElement.GetInt32() : 0;
            var width = existingData.ContainsKey("Width") && existingData["Width"] is JsonElement widthElement ? widthElement.GetInt32() : 0;
            var depth = existingData.ContainsKey("Depth") && existingData["Depth"] is JsonElement depthElement ? depthElement.GetInt32() : 0;

            // Créer un dictionnaire pour le locker à ajouter
            var lockerData = new Dictionary<string, object>
            {
                { "Couleur", SelectedCouleur ?? "" },  // Sauvegarder la couleur sélectionnée
                { "Longueur", Longueur },
                { "HasPorte", HasPorte },
                { "CouleurPorte", CouleurPorteSelected ?? "" },
                { "MaterielPorte", MatérielPorte ?? "" }
            };

            // Ajouter ce locker à la liste des lockers existants (si elle existe)
            if (existingData.ContainsKey("LockersData"))
            {
                lockersList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializer.Serialize(existingData["LockersData"])) ?? new List<Dictionary<string, object>>();
            }

            lockersList.Add(lockerData);  // Ajouter le nouveau locker

            // Mettre à jour les données existantes avec les nouvelles informations
            existingData["LockersData"] = lockersList;
            existingData["Height"] = height;
            existingData["Width"] = width;
            existingData["Depth"] = depth;
            existingData["Lockers"] = lockersList.Count;

            // Sauvegarder toutes les données mises à jour dans le fichier JSON
            string newJson = JsonSerializer.Serialize(existingData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, newJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'enregistrement : {ex.Message}");
        }
    }

    private void DeleteLockerData(int lockerIndex)
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
}
