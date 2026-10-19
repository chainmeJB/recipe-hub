using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {

            builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
            builder.Property(r => r.Description).HasMaxLength(1000);
            builder.Property(r => r.Instructions).IsRequired();

            builder.HasOne(r => r.Author)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}