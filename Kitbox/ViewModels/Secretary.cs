using CommunityToolkit.Mvvm.ComponentModel;
using Kitbox.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace Kitbox.ViewModels
{
    [ObservableObject]
    public partial class Secretary : User
    {
        public ObservableCollection<Supplier> Suppliers { get; private set; } = new();
        public ObservableCollection<Product> Products { get; private set; } = new();
        public List<string> Status { get; } = new() { "In Stock", "To Order", "Ordered", "Out Of Stock" };

        private readonly string filePath = "secretarydata.json";

        public Secretary()
        {
            LoadData(); // Charger les données existantes au démarrage
        }

        // Ajouter un fournisseur
        public void AddSupplier(int id, string name)
        {
            var supplier = new Supplier(id, name);
            Suppliers.Add(supplier);
            SaveData();
        }

        // Ajouter un produit
        public void AddProduct(int id, string name, decimal price, int deliveryTime, int supplierId)
        {
            var product = new Product(id, name, price, deliveryTime, supplierId);
            Products.Add(product);
            SaveData();
        }

        // Trier les produits par prix
        public void SortProductsByPrice()
        {
            var sortedProducts = Products.OrderBy(p => p.Price).ToList();
            Products.Clear();
            foreach (var product in sortedProducts)
                Products.Add(product);
        }

        // Trier les produits par délai de livraison
        public void SortProductsByDeliveryTime()
        {
            var sortedProducts = Products.OrderBy(p => p.DeliveryTime).ToList();
            Products.Clear();
            foreach (var product in sortedProducts)
                Products.Add(product);
        }

        // Sauvegarde dans un fichier JSON
        private void SaveData()
        {
            var data = new
            {
                Suppliers = Suppliers.ToList(),
                Products = Products.ToList()
            };
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }

        // Chargement des données depuis JSON
        private void LoadData()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedData = JsonSerializer.Deserialize<SecretaryData>(json);

                if (loadedData != null)
                {
                    Suppliers = new ObservableCollection<Supplier>(loadedData.Suppliers);
                    Products = new ObservableCollection<Product>(loadedData.Products);
                }
            }
        }
    }

    // Classe pour la désérialisation des données
    public class SecretaryData
    {
        public List<Supplier> Suppliers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
