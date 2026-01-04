using System;
using System.Collections.Generic;

namespace KioskApp.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
        public DateTime OrderTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
    
    public enum PaymentMethod
    {
        Card,
        Cash
    }
}
