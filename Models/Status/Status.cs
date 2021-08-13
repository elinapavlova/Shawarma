using System.Collections.Generic;

namespace Models.Status
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Order.Order> Orders { get; set; }
    }
}