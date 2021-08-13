using System;
using System.Collections.Generic;

namespace Models.Order
{
    public class OrderResponseDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public decimal Cost { get; set; }
        
        public int IdStatus { get; set; }
        public long IdUser { get; set; }
        
      //  public Status.StatusResponseDto Status { get; set; }
      //  public User.UserResponseDto User { get; set; }
        
      //  public List<OrderShawarma.OrderShawarmaResponseDto> OrderShawarma { get; set; }
    }
}