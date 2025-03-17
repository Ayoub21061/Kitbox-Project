using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Kitbox.ViewModels
{
    [ObservableObject]
    public partial class Secretary : User
    {
        // Liste des fournisseurs
        public List<Supplier> Suppliers { get; private set; } = new List<Supplier>();

        // Liste des produits
        public List<Product> Products { get; private set; } = new List<Product>();

        public Secretary() { }

        // Ajouter un fournisseur
        public void AddSupplier(int supplierId, string supplierName)
        {
            Suppliers.Add(new Supplier(supplierId, supplierName));
        }

        // Ajouter un produit
        public void AddProduct(int productId, string name, decimal price, int deliveryTime, int supplierId)
        {
            var supplier = Suppliers.FirstOrDefault(s => s.SupplierId == supplierId);
            if (supplier != null)
            {
                var newProduct = new Product(productId, name, price, deliveryTime, supplier);
                Products.Add(newProduct);
                supplier.Supplies.Add(newProduct); // Ajouter le produit à la liste du fournisseur
            }
        }

        // Trier les produits par prix
        public void SortProductsByPrice()
        {
            Products = Products.OrderBy(p => p.Price).ToList();
        }

        // Trier les produits par délai de livraison
        public void SortProductsByDeliveryTime()
        {
            Products = Products.OrderBy(p => p.DeliveryTime).ToList();
        }
    }

    // Modèle Supplier
    public class Supplier
    {
        public int SupplierId { get; }
        public string SupplierName { get; }
        public List<Product> Supplies { get; } = new List<Product>();

        public Supplier(int supplierId, string supplierName)
        {
            SupplierId = supplierId;
            SupplierName = supplierName;
        }
    }

    // Modèle Product
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; private set; }
        public int DeliveryTime { get; private set; }
        public Supplier Supplier { get; private set; }

        public Product(int productId, string name, decimal price, int deliveryTime, Supplier supplier)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            DeliveryTime = deliveryTime;
            Supplier = supplier;
        }
    }
}
