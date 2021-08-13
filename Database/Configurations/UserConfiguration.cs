using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.User;

namespace Database.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").Property(p => p.Id).HasDefaultValue(1).IsRequired();
            builder.ToTable("Users").Property(p => p.Email).IsRequired();
            builder.ToTable("Users").Property(p => p.Password).IsRequired();
            builder.ToTable("Users").Property(p => p.UserName).IsRequired();

            builder.ToTable("Users")
                .HasOne(p => p.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(p => p.IdRole);
        }
    }
}