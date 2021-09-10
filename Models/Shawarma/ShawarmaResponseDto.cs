
namespace Models.Shawarma
{
    public class ShawarmaResponseDto : BaseModel
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
    }
}