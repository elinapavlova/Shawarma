using System;
using System.Collections.Generic;

namespace Models.Order
{
    public class OrderResponseDto : BaseModel
    {
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int IdStatus { get; set; }
        public int IdUser { get; set; }

        public List<OrderShawarma.OrderShawarmaResponseDto> OrderShawarmas { get; set; }
    }
}