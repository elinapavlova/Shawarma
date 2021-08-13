using System;
using Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Models.Order;
using Models.OrderShawarma;
using Models.Role;
using Models.Shawarma;
using Models.Status;
using Models.User;

namespace Database
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shawarma> Shawarmas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<OrderShawarma> OrderShawarmas { get; set; }
        public DbSet<Role> Roles { get; set; }

        //public ApiContext()  { }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=shawarmadb;Username=postgres;Password=root");
        }
*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ShawarmaConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new OrderShawarmaConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}