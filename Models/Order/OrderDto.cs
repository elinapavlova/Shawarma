using System;

namespace Models.Order
{
    public class OrderDto : BaseModel
    {
        public DateTime Date { get; set; }
        public int IdStatus { get; set; }
        public int IdUser { get; set; }
        public decimal Cost { get; set; }
    }
}