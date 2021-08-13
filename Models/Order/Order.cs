using System;
using System.Collections.Generic;

namespace Models.Order
{
    public class Order
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int IdStatus { get; set; }
        public long IdUser { get; set; }
        public decimal Cost { get; set; }
        
        public Status.Status Status { get; set; }
        public User.User User { get; set; }
        
        public List<OrderShawarma.OrderShawarma> OrderShawarma { get; set; }
    }
}