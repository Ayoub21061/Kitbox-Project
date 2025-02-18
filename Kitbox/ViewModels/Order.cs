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
        protected string _orderDate;
        protected static readonly Regex DateRegex = new Regex(@"^\d{2}/\d{2}/\d{4}$");
        public string OrderDate
        {
            get => _orderDate;
            set
            {
                if (!DateRegex.IsMatch(value))
                    throw new ArgumentException("Format de date invalide. Utilisez dd/MM/yyyy.");

                _orderDate = value;
            }
        }


        public enum OrderStatusEnum { Awaiting_Stock, In_Progress, Saved, Ready_To_Go, Done}
        public OrderStatusEnum OrderStatus { get; protected set; }


        public enum PaymentStatusEnum { Partially_Paid, Totally_Paid, Unpaid}
        public PaymentStatusEnum PaymentStatus { get; protected set; }
        public decimal TotalAmount { get; protected set; } 

        public List<Product> Items { get; } = new List<Product>();
        protected Order(int orderId, string customerMail, string orderDate, decimal totalAmount)
        {
            OrderId = orderId;
            CustomerMail = customerMail;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            OrderStatus = OrderStatusEnum.Pending; // Par défaut
            PaymentStatus = PaymentStatusEnum.Pending; // Par défaut
        }



    }

}
