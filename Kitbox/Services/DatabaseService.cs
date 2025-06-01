using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Kitbox.Models;
using System.Data;

        public List<Stock> GetStocks()
        {
            var stocks = new List<Stock>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var query = @"SELECT 
                    product_id, article_name, article_status, article_quantity 
                  FROM Stock";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                stocks.Add(new Stock
                {
                    ProductId = reader.GetString("product_id"),
                    ArticleName = reader.GetString("article_name"),
                    ArticleStatus = reader.GetString("article_status"),
                    ArticleQuantity = reader.IsDBNull("article_quantity") ? null : reader.GetInt32("article_quantity")

                });
            }

            Console.WriteLine($"GetStocks : {stocks.Count} éléments récupérés.");
            return stocks;
        }


        public void AddStock(Stock stock)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var query = @"INSERT INTO Stock (product_id, article_name, article_status, article_quantity)
                  VALUES (@ProductId, @ArticleName, @ArticleStatus, @ArticleQuantity)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductId", stock.ProductId);
            cmd.Parameters.AddWithValue("@ArticleName", stock.ArticleName);
            cmd.Parameters.AddWithValue("@ArticleStatus", stock.ArticleStatus);
            cmd.Parameters.AddWithValue("@ArticleQuantity", (object?)stock.ArticleQuantity ?? DBNull.Value);

            cmd.ExecuteNonQuery();
            Console.WriteLine($"AddStock : Stock {stock.ProductId} ajouté.");
        }

        public void UpdateStock(Stock stock)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var query = @"UPDATE Stock 
                  SET article_name = @ArticleName,
                      article_status = @ArticleStatus,
                      article_quantity = @ArticleQuantity
                  WHERE product_id = @ProductId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductId", stock.ProductId);
            cmd.Parameters.AddWithValue("@ArticleName", stock.ArticleName);
            cmd.Parameters.AddWithValue("@ArticleStatus", stock.ArticleStatus);
            cmd.Parameters.AddWithValue("@ArticleQuantity", (object?)stock.ArticleQuantity ?? DBNull.Value);

            cmd.ExecuteNonQuery();
            Console.WriteLine($"UpdateStock : Stock {stock.ProductId} mis à jour.");
        }

        public void DeleteStock(string productId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            // Supprimer les références dans Supplier_Product
            var deleteSupplierProductQuery = @"DELETE FROM Supplier_Product WHERE product_id = @ProductId";
            using (var cmd = new MySqlCommand(deleteSupplierProductQuery, conn))
            {
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            }

            // Puis supprimer le produit dans Stock
            var deleteStockQuery = @"DELETE FROM Stock WHERE product_id = @ProductId";
            using (var cmd = new MySqlCommand(deleteStockQuery, conn))
            {
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"DeleteStock : Stock {productId} supprimé.");
        }
    }
}
