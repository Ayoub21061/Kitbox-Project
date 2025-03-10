using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Avalonia.Data;
using Kitbox.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Supplier> suppliers = new List<Supplier>(); // Liste des fournisseurs

        [ObservableProperty]
        private string supplierName;

        [ObservableProperty]
        private decimal supplierPrice;

        [ObservableProperty]
        private int supplierDeliveryDays;

        // Commande pour ajouter un fournisseur
        public IRelayCommand AddSupplierCommand { get; }

        // Commande pour trier les fournisseurs par prix
        public IRelayCommand SortByPriceCommand { get; }

        // Commande pour trier les fournisseurs par délai de livraison
        public IRelayCommand SortByDeliveryDaysCommand { get; }

        public SecretaryViewModel()
        {
            AddSupplierCommand = new RelayCommand(AddSupplier);
            SortByPriceCommand = new RelayCommand(SortSuppliersByPrice);
            SortByDeliveryDaysCommand = new RelayCommand(SortSuppliersByDeliveryDays);
        }

        private void AddSupplier()
        {
            var newSupplier = new Supplier
            {
                SupplierName = SupplierName,
                Price = SupplierPrice,
                DeliveryDays = SupplierDeliveryDays
            };

            Suppliers.Add(newSupplier); // Ajouter le fournisseur à la liste
        }

        private void SortSuppliersByPrice()
        {
            Suppliers = Suppliers.OrderBy(s => s.Price).ToList(); // Trier par prix
        }

        private void SortSuppliersByDeliveryDays()
        {
            Suppliers = Suppliers.OrderBy(s => s.DeliveryDays).ToList(); // Trier par délai de livraison
        }
    }
}
