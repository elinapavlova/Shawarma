﻿namespace Models.OrderShawarma
{
    public class OrderShawarmaResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
        
       // public Shawarma.ShawarmaResponseDto Shawarma { get; set; }
       // public Order.OrderResponseDto Order { get; set; }
    }
}