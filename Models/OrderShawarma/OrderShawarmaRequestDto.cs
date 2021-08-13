namespace Models.OrderShawarma
{
    public class OrderShawarmaRequestDto
    {
        public long OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
        
        // public Shawarma.ShawarmaRequestDto Shawarma { get; set; }
        // public Order.OrderRequestDto Order { get; set; }
    }
}