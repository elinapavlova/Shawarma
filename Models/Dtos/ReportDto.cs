using System;
using System.Collections.Generic;

namespace Models.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public DateTime WasCreated { get; set; }
        public List<ReportOrder> Orders { get; set; } 
    }
}