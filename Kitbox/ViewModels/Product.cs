using System;

namespace Kitbox.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DeliveryTime { get; set; }
        public int SupplierId { get; set; }

        public Product(int id, string name, decimal price, int deliveryTime, int supplierId)
        {
            ProductId = id;
            Name = name;
            Price = price;
            DeliveryTime = deliveryTime;
            SupplierId = supplierId;
        }
    }
}
