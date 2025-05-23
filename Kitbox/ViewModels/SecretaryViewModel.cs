
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
<<<<<<< HEAD
        public  Secretary _secretary;
=======
        // private Secretary _secretary;

        /*
        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers;

        [ObservableProperty]
        private ObservableCollection<Product> products;
>>>>>>> origin/main

        public IRelayCommand SecondSecretaryPageCommand { get; }
        public IRelayCommand SaveSecretaryCommand { get; }

        public SecretaryViewModel()
        {
            _secretary = new Secretary();

            SecondSecretaryPageCommand = new RelayCommand(SecondNextPageSec);
            //SaveSecretaryCommand = new RelayCommand(SaveSecretaryDataToJson); // Initialisation
        }

<<<<<<< HEAD
        // Naviguer vers la deuxième page
        private void SecondNextPageSec()
        {
            var secondPage = new SecondSecretaryPageView();
            secondPage.Show();
        }
    
=======
        private bool CanExecuteNextPage()
        {
            return true; // Si tu veux que la commande soit toujours activée
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
        */

        // private void SecondNextPage()
        // {
        //     var SecondPage = new SecondSecretaryPageView();
        //     SecondPage.Show();
        // }
>>>>>>> origin/main
    }
}
