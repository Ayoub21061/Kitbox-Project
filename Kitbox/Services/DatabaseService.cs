using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Kitbox.Models;

namespace TonProjet.Services
{
    public class DatabaseService
    {
        private readonly string connectionString =
            "server=2001:6a8:11d0:11::152;port=3306;user=groupe;password=motdepassefort;database=ma_base;";

        public bool TesterConnexion()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Connexion réussie !");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
        }

        public List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new();

            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                var query = "SELECT supplier_id, name FROM Suppliers;"; // ✅ Table 'Suppliers' avec majuscule
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["supplier_id"]);
                    string name = reader["name"].ToString();

                    suppliers.Add(new Supplier(id, name));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du chargement des fournisseurs : " + ex.Message);
            }

            return suppliers;
        }

        public void Addsuppliers(Supplier supplier)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                conn.Open();
                var query = "INSERT INTO Suppliers (name) VALUES (@name);"; // ✅ Table 'Suppliers'
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", supplier.SupplierName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout d'un fournisseur : " + ex.Message);
            }
        }
    }
}
