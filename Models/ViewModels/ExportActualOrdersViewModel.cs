using System;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class ExportActualOrdersViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        
        public List<OrderShawarmaViewModel> OrderShawarmas { get; set; } 
    }
}