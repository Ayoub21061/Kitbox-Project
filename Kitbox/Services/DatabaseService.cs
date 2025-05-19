using System;
using MySql.Data.MySqlClient;

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
    }
}
