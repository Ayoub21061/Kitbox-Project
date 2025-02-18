using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kitbox.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        public Customer customer; // Champ privé de type Customer.

        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private int width;

        [ObservableProperty]
        private int depth;

        [ObservableProperty]
        private int doorCount;

        [ObservableProperty]
        private int shelfCount;

        [ObservableProperty]
        private string color;

        // Liste des styles de portes disponibles
        [ObservableProperty]
        private ObservableCollection<DoorStyle> availableDoorStyles;

        // Liste des portes sélectionnables en fonction du nombre de portes
        public ObservableCollection<DoorStyle> DoorSelectionList
        {
            get
            {
                // Afficher les portes seulement si DoorCount >= 1
                if (DoorCount >= 1)
                {
                    var count = Math.Min(DoorCount, AvailableDoorStyles.Count);
                    return new ObservableCollection<DoorStyle>(AvailableDoorStyles.Take(count));
                }
                else
                {
                    // Si DoorCount est 0, renvoyer une liste vide
                    return new ObservableCollection<DoorStyle>();
                }
            }
        }

        public CustomerViewModel() // Constructeur qui initialise Id et Email avec les valeurs passées en paramètre. Pour créer un client directement avec Id et email.
        {
            Customer = new Customer(); // Permet d'obtenir une référence à un objet Customer.
            Height = 0;
            Width = 0;
            Depth = 0;
            DoorCount = 0;
            ShelfCount = 0;
            Color = string.Empty;

            // Initialisation de la liste des styles de portes
            AvailableDoorStyles = new ObservableCollection<DoorStyle>
            {
                new DoorStyle("Style 1"),
                new DoorStyle("Style 2"),
                new DoorStyle("Style 3"),
                new DoorStyle("Style 4")
            };
        }
    }
}
