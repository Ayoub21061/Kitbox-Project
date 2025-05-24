using System;

namespace Kitbox.Models
{
    public class Order_Client
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string StatusDelivery { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Deposit { get; set; }
        public DateTime DateCommandClient { get; set; }
    }
}
