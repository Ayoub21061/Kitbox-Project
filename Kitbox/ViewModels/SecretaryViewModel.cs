using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using Kitbox.Models;
using System.Collections.ObjectModel;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        public Secretary _secretary;
        public string Message { get; set; }

        public ObservableCollection<Supplier> Suppliers { get; } = new();
        public ObservableCollection<Product> SupplierProducts { get; } = new();

        public IRelayCommand SecondSecretaryPageCommand { get; }

        private readonly string _connectionString = "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

        public SecretaryViewModel()
        {
            _secretary = new Secretary();
            SecondSecretaryPageCommand = new RelayCommand(SecondNextPageSec);

            // Test de connexion
            if (TestConnection())
            {
                Message = "✅ Connexion à la base réussie";
                LoadSuppliers();
            }
            else
            {
                Message = "❌ Connexion échouée";
            }
        }

        private void SecondNextPageSec()
        {
            var secondPage = new SecondSecretaryPageView();
            secondPage.Show();
        }

        private bool TestConnection()
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Requête : Get all suppliers
        public void LoadSuppliers()
        {
            Suppliers.Clear();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT supplier_id, name FROM suppliers ORDER BY name";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Suppliers.Add(new Supplier(
                    reader.GetInt32("supplier_id"),
                    reader.GetString("name")));
            }
        }

        // Requête : Get all products for all suppliers
        public void LoadSupplierProducts()
        {
            SupplierProducts.Clear();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM Supplier_Product";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                SupplierProducts.Add(new Product(
                    reader.GetInt32("product_id"),
                    reader.GetString("name"),
                    reader.GetDecimal("price"),
                    reader.GetInt32("delivery_time"),
                    reader.GetInt32("supplier_id")));
            }
        }

        // Requête : Add new supplier
        public void AddSupplier(string name)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO suppliers (name) VALUES (@name)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
        }

        // Requête : Add new product for a supplier
        public void AddSupplierProduct(string name, decimal price, int deliveryTime, int supplierId)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO Supplier_Product (name, price, delivery_time, supplier_id) VALUES (@name, @price, @deliveryTime, @supplierId)";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@deliveryTime", deliveryTime);
            cmd.Parameters.AddWithValue("@supplierId", supplierId);
            cmd.ExecuteNonQuery();
        }

        // Requête : Search supplier by name
        public ObservableCollection<Supplier> SearchSupplierByName(string name)
        {
            var result = new ObservableCollection<Supplier>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM suppliers WHERE name LIKE @name";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Supplier(
                    reader.GetInt32("supplier_id"),
                    reader.GetString("name")));
            }

            return result;
        }

        // Requête : Search supplier by ID
        public Supplier? SearchSupplierById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM suppliers WHERE supplier_id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Supplier(reader.GetInt32("supplier_id"), reader.GetString("name"));
            }

            return null;
        }

        // Requête : Search supplier_product by name
        public ObservableCollection<Product> SearchSupplierProductByName(string name)
        {
            var result = new ObservableCollection<Product>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM Supplier_Product WHERE name LIKE @name";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Product(
                    reader.GetInt32("product_id"),
                    reader.GetString("name"),
                    reader.GetDecimal("price"),
                    reader.GetInt32("delivery_time"),
                    reader.GetInt32("supplier_id")));
            }

            return result;
        }

        // Requête : Search supplier_product by ID
        public Product? SearchSupplierProductById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM Supplier_Product WHERE product_id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Product(
                    reader.GetInt32("product_id"),
                    reader.GetString("name"),
                    reader.GetDecimal("price"),
                    reader.GetInt32("delivery_time"),
                    reader.GetInt32("supplier_id"));
            }

            return null;
        }

        // Requête : Search supplier_product by delivery status (e.g., delivery_time)
        public ObservableCollection<Product> SearchSupplierProductByDeliveryTime(int maxDays)
        {
            var result = new ObservableCollection<Product>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT * FROM Supplier_Product WHERE delivery_time <= @maxDays";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@maxDays", maxDays);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Product(
                    reader.GetInt32("product_id"),
                    reader.GetString("name"),
                    reader.GetDecimal("price"),
                    reader.GetInt32("delivery_time"),
                    reader.GetInt32("supplier_id")));
            }

            return result;
        }
    }
}
