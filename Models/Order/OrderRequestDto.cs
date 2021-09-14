using System;

namespace Models.Order
{
    public class OrderRequestDto : BaseModel
    {
        public DateTime Date = DateTime.Now;
        public string Comment { get; set; }
        public int IdStatus { get; set; }
        public int IdUser { get; set; }
    }
}