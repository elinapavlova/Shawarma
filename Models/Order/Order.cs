using System;
using System.Collections.Generic;

namespace Models.Order
{
    public class Order : BaseModel
    {
        public DateTime Date { get; set; }
        public int IdStatus { get; set; }
        public int IdUser { get; set; }
        public decimal Cost { get; set; }

        public Status.Status Status { get; set; }
        public User.User User { get; set; }
        
        public List<OrderShawarma.OrderShawarma> OrderShawarmas { get; set; }
    }
}