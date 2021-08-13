using System;
using System.Collections.Generic;

namespace Models.Order
{
    public class OrderRequestDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public decimal Cost { get; set; }
        
        public int IdStatus { get; set; }
        public long IdUser { get; set; }
        
      //  public Status.StatusRequestDto Status { get; set; }
      //  public User.UserRequestDto User { get; set; }
        
      //  public List<OrderShawarma.OrderShawarmaRequestDto> OrderShawarma { get; set; }
    }
}