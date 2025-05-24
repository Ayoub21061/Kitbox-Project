using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Kitbox.Models;

namespace Kitbox.Services
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

        public List<Order_Client> GetOrders()
        {
            var orders = new List<Order_Client>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var query = @"SELECT 
                    order_id, client_id, status_delivery, payment_status, 
                    total_amount, deposit, date_command_client 
                  FROM Order_Client";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order_Client
                {
                    OrderId = reader.GetInt32("order_id"),
                    ClientId = reader.GetInt32("client_id"),
                    StatusDelivery = reader.GetString("status_delivery"),
                    PaymentStatus = reader.GetString("payment_status"),
                    TotalAmount = reader.GetDecimal("total_amount"),
                    Deposit = reader.GetDecimal("deposit"),
                    DateCommandClient = reader.GetDateTime("date_command_client")
                });
            }

            Console.WriteLine($"GetOrders : {orders.Count} ordres récupérés."); // <-- ajout console

            return orders;
        }

        public void AddOrderClient(Order_Client order)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var query = @"INSERT INTO Order_Client 
                (order_id, client_id, status_delivery, payment_status, total_amount, deposit, date_command_client) 
                VALUES (@OrderId, @ClientId, @StatusDelivery, @PaymentStatus, @TotalAmount, @Deposit, @DateCommandClient)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
            cmd.Parameters.AddWithValue("@ClientId", order.ClientId);
            cmd.Parameters.AddWithValue("@StatusDelivery", order.StatusDelivery);
            cmd.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus);
            cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            cmd.Parameters.AddWithValue("@Deposit", order.Deposit);
            cmd.Parameters.AddWithValue("@DateCommandClient", order.DateCommandClient);

            cmd.ExecuteNonQuery();
        }
    }
}
