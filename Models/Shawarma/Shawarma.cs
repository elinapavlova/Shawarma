using System.Collections.Generic;

namespace Models.Shawarma
{
    public class Shawarma : BaseModel
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
        
        public List<OrderShawarma.OrderShawarma> OrderShawarmas { get; set; }
    }
}