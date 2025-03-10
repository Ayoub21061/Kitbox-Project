using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kitbox.ViewModels

{
    [ObservableObject]
    public partial class Secretary : User

    {
        [ObservableProperty]
        private string? confirm;

        // Liste des fournisseurs
        public List<Supplier> Suppliers { get; private set; } = new List<Supplier>();

        // Liste des produits
        public List<Product> Products { get; private set; } = new List<Product>();

        public Secretary()
        {

        }

        public void AddSupplier(int supplierId, string supplierName)
        {
            Suppliers.Add(new Supplier(supplierId, supplierName));
        }

        public void AddProduct(int productId, string name, decimal price, int deliveryTime, int supplierId)
        {
            var supplier = Suppliers.FirstOrDefault(s => s.SupplierId == supplierId);
            // Créer un nouveau produit et l'ajouter
            var newProduct = new Product(productId, name, price, deliveryTime, supplier);
            Products.Add(newProduct);
            supplier.Supplies.Add(newProduct); // Ajouter le produit à la liste du fournisseur
                    
        }

        
        // produits trier par prix (moins cher au plus cher)
        public void SortProductsByPrice()
        {
            Products = Products.OrderBy(p => p.Price).ToList();
        }

        // produit trier par délai de livraison (plus rapide au plus lent)
        public void SortProductsByDeliveryTime()
        {
            Products = Products.OrderBy(p => p.DeliveryTime).ToList();
        }
    }


    public class Supplier
    {
        public int SupplierId { get; private set; }
        public string SupplierName { get; private set; }
        public List<Product> Supplies { get; private set; } = new List<Product>();

        public Supplier(int supplierId, string supplierName)
        {
            SupplierId = supplierId;
            SupplierName = supplierName;
        }
    }

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