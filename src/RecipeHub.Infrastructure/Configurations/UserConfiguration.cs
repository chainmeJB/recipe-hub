using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace RecipeHub.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(100);

            builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        }
    }
}
