using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Order;

namespace Database.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders").Property(p => p.Id).IsRequired();
            builder.ToTable("Orders").Property(p => p.Date).IsRequired();
            builder.ToTable("Orders").Property(p => p.Comment).HasMaxLength(300);

            builder.ToTable("Orders")
                .HasOne(p => p.User)
                .WithMany(t => t.Orders)
                .HasForeignKey(p => p.IdUser);
            
            builder.ToTable("Orders")
                .HasOne(p => p.Status)
                .WithMany(t => t.Orders)
                .HasForeignKey(p => p.IdStatus);


        }
    }
}