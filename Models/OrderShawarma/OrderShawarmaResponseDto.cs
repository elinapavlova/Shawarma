namespace Models.OrderShawarma
{
    public class OrderShawarmaResponseDto : BaseModel
    {
        public int OrderId { get; set; }
        public int ShawarmaId { get; set; }
        public int Number { get; set; }
    }
}