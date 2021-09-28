using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.ViewModels;

namespace Database
{
    public class Context : DbContext
    {
        public DbSet<ShawarmaDto> Shawarmas { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportOrder> ReportOrders { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportOrder>(p =>
            {
                p.HasOne(reportOrder => reportOrder.Report)
                    .WithMany(r => r.ReportOrders)
                    .HasForeignKey(reportOrder => reportOrder.ReportId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                p.HasOne(reportOrder => reportOrder.Order)
                    .WithMany(r => r.ReportOrders)
                    .HasForeignKey(reportOrder => reportOrder.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<OrderShawarmaViewModel>(p => p.HasNoKey());
        }
    }
}