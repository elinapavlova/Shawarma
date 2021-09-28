using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Models.ViewModels;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        
        [NotMapped]
        public List<OrderShawarmaViewModel> OrderShawarmas { get; set; }
    }
}