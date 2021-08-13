using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Role;

namespace Database.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles").Property(p => p.Id).IsRequired();
            builder.ToTable("Roles").Property(p => p.Name).IsRequired();
        }
    }
}