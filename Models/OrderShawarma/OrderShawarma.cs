namespace Models.OrderShawarma
{
    public class OrderShawarma
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
        
        public Shawarma.Shawarma Shawarma { get; set; }
        public Order.Order Order { get; set; }
        
    }
}