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
        public string Message { get; set; }

        public Secretary _secretary;

        public ObservableCollection<Supplier> Suppliers { get; set; } = new();

        public IRelayCommand SecondSecretaryPageCommand { get; }
        public IRelayCommand? SaveSecretaryCommand { get; }

        public IRelayCommand AddSupplierCommand { get; } // 🔹 Déclaration ici

        public SecretaryViewModel()
        {
            _secretary = new Secretary();

            SecondSecretaryPageCommand = new RelayCommand(SecondNextPageSec);
            // SaveSecretaryCommand = new RelayCommand(SaveSecretaryDataToJson);
            AddSupplierCommand = new RelayCommand(AddSupplier); // ✅ Initialisation ici

            var db = new TonProjet.Services.DatabaseService();
            Message = db.TesterConnexion()
                ? "✅ Connexion à la base réussie"
                : "❌ Connexion échouée";

            var fournisseurs = db.GetSuppliers();
            foreach (var f in fournisseurs)
            {
                Suppliers.Add(f);
            }

        }

        private void SecondNextPageSec()
        {
            var secondPage = new SecondSecretaryPageView();
            secondPage.Show();
        }

        private void AddSupplier()
        {
            var nouveau = new Supplier(0, "New Supplier"); // ou récupère un champ de saisie
            Suppliers.Add(nouveau);

            var db = new TonProjet.Services.DatabaseService();
            db.Addsuppliers(nouveau);
        }

    }
}
