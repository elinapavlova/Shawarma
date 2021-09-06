
using System.ComponentModel.DataAnnotations;

namespace Models.Shawarma
{
    public class ShawarmaRequestDto
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActual { get; set; }
    }
}