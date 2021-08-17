using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.OrderShawarma;

namespace Database.Configurations
{
    public class OrderShawarmaConfiguration : IEntityTypeConfiguration<OrderShawarma>
    {
        public void Configure(EntityTypeBuilder<OrderShawarma> builder)
        {
            builder.ToTable("OrderShawarma").Property(p => p.Id).IsRequired();
            builder.ToTable("OrderShawarma").Property(p => p.Number).HasDefaultValue(1).IsRequired();
           
            builder.ToTable("OrderShawarma")
                .HasOne(p => p.Order)
                .WithMany(t => t.OrderShawarmas)
                .HasForeignKey(p => p.OrderId);
            
            builder.ToTable("OrderShawarma")
                .HasOne(p => p.Shawarma)
                .WithMany(t => t.OrderShawarmas)
                .HasForeignKey(p => p.ShawarmaId);

        }
    }
}