using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using RecipeHub.Infrastructure.Entities;

namespace RecipeHub.Infrastructure.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {

            builder.Property(i => i.Url).IsRequired().HasMaxLength(2048);

            builder.HasOne(i => i.Recipe)
                .WithMany(r => r.Images)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
