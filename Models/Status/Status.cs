using System.Collections.Generic;

namespace Models.Status
{
    public class Status : BaseModel
    {
        public string Name { get; set; }
        public List<Order.Order> Orders { get; set; }
    }
}