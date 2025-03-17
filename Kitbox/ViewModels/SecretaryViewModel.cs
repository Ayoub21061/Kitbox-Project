using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System.Collections.Generic;
using System.Linq;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        // Instance de la classe Secretary pour accéder à la logique métier
        private Secretary _secretary = new Secretary(1, "Secretary Name"); 

        public class Secretary
{
    // Liste des fournisseurs
    public List<Supplier> Suppliers { get; private set; } = new List<Supplier>();

    // Liste des produits
    public List<Product> Products { get; private set; } = new List<Product>();

    // Constructeur avec ID et nom
    public Secretary(int id, string name)
    {
        // Initialisation de l'ID et du nom si nécessaire
    }

    // Ajouter un fournisseur
    public void AddSupplier(int supplierId, string supplierName)
    {
        Suppliers.Add(new Supplier(supplierId, supplierName));
    }

    // Ajouter un produit lié à un fournisseur
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



        // Propriétés pour lier à l'interface (liste des produits, fournisseurs, etc.)
        [ObservableProperty]
        public List<Supplier> suppliers = [];

        [ObservableProperty]
        public List<Product> products = [];

        // Ajouter un fournisseur
        public void AddSupplier(int supplierId, string supplierName)
        {
            _secretary.AddSupplier(supplierId, supplierName);
            Suppliers = _secretary.Suppliers;
        }

        // Ajouter un produit
        public void AddProduct(int productId, string name, decimal price, int deliveryTime, int supplierId)
        {
            _secretary.AddProduct(productId, name, price, deliveryTime, supplierId);
            Products = _secretary.Products;
        }

        // Trier les produits par prix
        public void SortProductsByPrice()
        {
            _secretary.SortProductsByPrice();
            Products = _secretary.Products;
        }

        // Trier les produits par délai de livraison
        public void SortProductsByDeliveryTime()
        {
            _secretary.SortProductsByDeliveryTime();
            Products = _secretary.Products;
        }
    }
}
