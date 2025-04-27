using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using Kitbox.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.IO;
using System;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        private readonly Secretary _secretary;

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers;

        [ObservableProperty]
        private ObservableCollection<Product> products;

        public IRelayCommand SecondSecretaryPageCommand { get; }
        public IRelayCommand AddSupplierCommand { get; }
        public IRelayCommand AddProductCommand { get; }
        public IRelayCommand SaveSecretaryCommand { get; } // Ajout de la commande

        public SecretaryViewModel()
        {
            _secretary = new Secretary();

            // Initialisation des collections
            Suppliers = new ObservableCollection<Supplier>(_secretary.Suppliers);
            Products = new ObservableCollection<Product>(_secretary.Products);

            // Commandes
            SecondSecretaryPageCommand = new RelayCommand(SecondNextPageSec);
            AddSupplierCommand = new RelayCommand(() => AddSupplier(1, "Default Supplier"));
            AddProductCommand = new RelayCommand(() => AddProduct(1, "Default Product", 100m, 5, 1));
            SaveSecretaryCommand = new RelayCommand(SaveSecretaryDataToJson); // Initialisation
        }

        // Ajouter un fournisseur
        public void AddSupplier(int supplierId, string supplierName)
        {
            var newSupplier = new Supplier(supplierId, supplierName);
            _secretary.AddSupplier(supplierId, supplierName);
            Suppliers.Add(newSupplier);
        }

        // Ajouter un produit
        public void AddProduct(int productId, string name, decimal price, int deliveryTime, int supplierId)
        {
            var newProduct = new Product(productId, name, price, deliveryTime, supplierId);
            _secretary.AddProduct(productId, name, price, deliveryTime, supplierId);
            Products.Add(newProduct);
        }

        // Trier les produits par prix
        public void SortProductsByPrice()
        {
            var sortedProducts = _secretary.Products.OrderBy(p => p.Price).ToList();
            Products.Clear();
            foreach (var product in sortedProducts)
                Products.Add(product);
        }

        // Trier les produits par délai de livraison
        public void SortProductsByDeliveryTime()
        {
            var sortedProducts = _secretary.Products.OrderBy(p => p.DeliveryTime).ToList();
            Products.Clear();
            foreach (var product in sortedProducts)
                Products.Add(product);
        }

        // Naviguer vers la deuxième page
        private void SecondNextPageSec()
        {
            var secondPage = new SecondSecretaryPageView();
            secondPage.Show();
        }

        // Sauvegarde en JSON
        private void SaveSecretaryDataToJson()
        {
            try
            {
                var secretaryData = new
                {
                    Suppliers = Suppliers,
                    Products = Products
                };

                string json = JsonSerializer.Serialize(secretaryData, new JsonSerializerOptions { WriteIndented = true });

                string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "KitboxData");
                Directory.CreateDirectory(directoryPath); // Créé le dossier si pas encore existant
                string filePath = Path.Combine(directoryPath, "SecretaryData.json");

                File.WriteAllText(filePath, json);

                // Optionnel : Message de succès
                Console.WriteLine($"Data saved successfully to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving data: {ex.Message}");
            }
        }
    }
}
