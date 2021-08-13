namespace Models.OrderShawarma
{
    public class OrderShawarma
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
        
        public Shawarma.Shawarma Shawarma { get; set; }
        public Order.Order Order { get; set; }
        
    }
}