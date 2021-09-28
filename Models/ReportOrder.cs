namespace Models
{
    public class ReportOrder
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int OrderId { get; set; }
        
        public Report Report { get; set; }
        public Order Order { get; set; }
    }
}