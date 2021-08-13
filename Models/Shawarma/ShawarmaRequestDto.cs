using System.Collections.Generic;

namespace Models.Shawarma
{
    public class ShawarmaRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
        
        //public List<OrderShawarma.OrderShawarmaRequestDto> OrderShawarma { get; set; }
    }
}