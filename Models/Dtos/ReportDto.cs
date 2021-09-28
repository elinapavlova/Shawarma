using System;

namespace Models.Dtos
{
    public class ReportDto
    {
        public int Id { get; set; }
        public DateTime WasCreated { get; set; }
        public string FileName { get; set; }
        public byte[] Document { get; set; }
    }
}