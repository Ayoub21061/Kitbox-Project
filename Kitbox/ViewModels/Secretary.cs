using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        private string? supplierName;

        [ObservableProperty]
        private int supplierId;

        [ObservableProperty]
        private string? articleName;

        [ObservableProperty]
        private string? dimensions;

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private decimal sellingPrice;

        [ObservableProperty]
        private decimal supplierPrice;

        [ObservableProperty]
        private int stockQuantity;

        [ObservableProperty]
        private string? bestSupplierPrice;

        [ObservableProperty]
        private string? bestSupplierTime;

        public ObservableCollection<dynamic> Suppliers { get; private set; } = new();
        public ObservableCollection<dynamic> Products { get; private set; } = new();
        public List<string> Status { get; } = new() { "In Stock", "To Order", "Ordered", "Out Of Stock" };

        private readonly string filePath = "secretarydata.json";

        public Secretary() 
        { 
            SupplierName = "";
            SupplierId = 0;
            ArticleName = "";
            Dimensions = "";
            ProductId = 0;
            SellingPrice = 0;
            SupplierPrice = 0;
            StockQuantity = 0;
            BestSupplierPrice = "";
            BestSupplierTime = "";
            LoadData(); // Charger les données existantes au démarrage
        }

        public Secretary(string supplierName, int supplierId, string articleName, string dimensions, int productId, decimal sellingPrice, decimal supplierPrice, int stockQuantity, string bestSupplierPrice, string bestSupplierTime)
        {
            SupplierName = supplierName;
            SupplierId = supplierId;
            ArticleName = articleName;
            Dimensions = dimensions;
            ProductId = productId;
            SellingPrice = sellingPrice;
            SupplierPrice = supplierPrice;
            StockQuantity = stockQuantity;
            BestSupplierPrice = bestSupplierPrice;
            BestSupplierTime = bestSupplierTime;
        }

        // Ajouter un fournisseur
        public void AddSupplier(int id, string name)
        {
            var supplier = new { SupplierId = id, SupplierName = name };
            Suppliers.Add(supplier);
            SaveData();
        }

        // Ajouter un produit
        public void AddProduct(int id, string name, decimal price, int deliveryTime, int supplierId)
        {
            var product = new { ProductId = id, Name = name, Price = price, DeliveryTime = deliveryTime, SupplierId = supplierId };
            Products.Add(product);
            SaveData();
        }

        // Trier les produits par prix
        public void SortProductsByPrice() =>
            Products = new ObservableCollection<dynamic>(Products.OrderBy(p => p.Price));

        // Trier les produits par délai de livraison
        public void SortProductsByDeliveryTime() =>
            Products = new ObservableCollection<dynamic>(Products.OrderBy(p => p.DeliveryTime));

         // Sauvegarde dans un fichier JSON
        private void SaveData()
        {
            var data = new { Suppliers, Products };
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }

        // Chargement des données depuis JSON
        private void LoadData()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedData = JsonSerializer.Deserialize<dynamic>(json);
                if (loadedData != null)
                {
                    Suppliers = new ObservableCollection<dynamic>(loadedData.Suppliers);
                    Products = new ObservableCollection<dynamic>(loadedData.Products);
                }
            }
        }
    }
}