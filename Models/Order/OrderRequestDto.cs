using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
    public class OrderRequestDto
    {
        public int Id { get; set; }
        
        public DateTime Date = DateTime.Now;
        public string Comment { get; set; }

        public int IdStatus { get; set; }
        public int IdUser { get; set; }
    }
}