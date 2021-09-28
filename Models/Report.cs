using System;

namespace Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime WasCreated { get; set; }
        public string FileName { get; set; }
        public byte[] Document { get; set; }
    }
}