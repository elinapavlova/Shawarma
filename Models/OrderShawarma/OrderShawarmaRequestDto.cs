namespace Models.OrderShawarma
{
    public class OrderShawarmaRequestDto : BaseModel
    {
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
    }
}