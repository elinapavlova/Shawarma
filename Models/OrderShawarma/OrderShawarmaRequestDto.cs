namespace Models.OrderShawarma
{
    public class OrderShawarmaRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
        
        // public Shawarma.ShawarmaRequestDto Shawarma { get; set; }
        // public Order.OrderRequestDto Order { get; set; }
    }
}