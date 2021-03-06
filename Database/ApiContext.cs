using Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.OrderShawarma;
using Models.Shawarma;
using Models.User;

namespace Database
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shawarma> Shawarmas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderShawarma> OrderShawarmas { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ShawarmaConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderShawarmaConfiguration());
        }
    }
}