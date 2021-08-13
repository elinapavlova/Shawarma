using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Status;

namespace Database.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses").Property(p => p.Id).IsRequired();
            builder.ToTable("Statuses").Property(p => p.Name).IsRequired();
        }
    }
}