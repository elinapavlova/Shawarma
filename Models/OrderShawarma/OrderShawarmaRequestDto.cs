namespace Models.OrderShawarma
{
    public class OrderShawarmaRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
    }
}