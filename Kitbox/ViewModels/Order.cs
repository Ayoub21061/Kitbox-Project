/*
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kitbox.ViewModels
{
    public abstract class Order
    {
        public int OrderId { get; protected set; } 
        public string CustomerMail { get; protected set; } 
        protected string? _orderDate;
        protected static readonly Regex DateRegex = new Regex(@"^\d{2}/\d{2}/\d{4}$");
        public DateTime OrderDate {get; protected set;}= DateTime.Now;


         public enum OrderStatusEnum { Awaiting_Stock, In_Progress, Saved, Ready_To_Go, Done}
         public OrderStatusEnum OrderStatus { get; protected set; }


         public enum PaymentStatusEnum { Partially_Paid, Totally_Paid, Unpaid}
         public PaymentStatusEnum PaymentStatus { get; protected set; }
         public decimal TotalAmount { get; protected set; } 

         public List<Product> Items { get; } = new List<Product>();
         protected Order(int orderId, string customerMail, DateTime orderDate, decimal totalAmount)
         {
             OrderId = orderId;
             CustomerMail = customerMail;
             OrderDate = orderDate;
             TotalAmount = totalAmount;
             OrderStatus = OrderStatusEnum.Saved; // Par défaut
             PaymentStatus = PaymentStatusEnum.Unpaid; // Par défaut
         }



     }

}
*/