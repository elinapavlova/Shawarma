
namespace Models.Shawarma
{
    public class ShawarmaRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
    }
}