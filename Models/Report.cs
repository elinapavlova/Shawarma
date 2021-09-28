using System;
using System.Collections.Generic;

namespace Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime WasCreated { get; set; }

        public List<ReportOrder> ReportOrders { get; set; } 
    }
}