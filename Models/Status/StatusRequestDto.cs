using System.Collections.Generic;

namespace Models.Status
{
    public class StatusRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Order.OrderRequestDto> Orders { get; set; }
    }
}