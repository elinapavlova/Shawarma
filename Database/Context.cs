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

        public Context(DbContextOptions<Context> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderShawarmaViewModel>(p => p.HasNoKey());
        }
    }
}