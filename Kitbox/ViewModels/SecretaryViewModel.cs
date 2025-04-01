using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System.Collections.Generic;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        private Secretary _secretary;

        [ObservableProperty]
        public List<Supplier> suppliers = new List<Supplier>();

        [ObservableProperty]
        public List<Product> products = new List<Product>();

        // Commande pour passer à la page suivante
        public IRelayCommand SecondSecretaryPageCommand { get; }

        public SecretaryViewModel()
        {
            _secretary = new Secretary();
            SecondSecretaryPageCommand = new RelayCommand(SecondNextPage);
        }

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

        private void SecondNextPage()
        {
            var SecondPage = new SecondSecretaryPageView();
            SecondPage.Show();
        }
    }
}
