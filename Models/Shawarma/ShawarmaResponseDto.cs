using System.Collections.Generic;

namespace Models.Shawarma
{
    public class ShawarmaResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
    }
}