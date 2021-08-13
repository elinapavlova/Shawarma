using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Shawarma;

namespace Database.Configurations
{
    public class ShawarmaConfiguration : IEntityTypeConfiguration<Shawarma>
    {
        public void Configure(EntityTypeBuilder<Shawarma> builder)
        {
            builder.ToTable("Shawarmas").Property(p => p.Id).IsRequired();
            builder.ToTable("Shawarmas").Property(p => p.Name).IsRequired();
            builder.ToTable("Shawarmas").Property(p => p.Cost).IsRequired();
            builder.ToTable("Shawarmas").Property(p => p.IsActual).HasDefaultValue(0).IsRequired();
        }
    }
}