using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Configurations
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {

            builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            builder.Property(ri => ri.Quantity).HasPrecision(10, 2);
            builder.Property(ri => ri.Unit).IsRequired().HasMaxLength(32);
            builder.Property(ri => ri.Note).HasMaxLength(200);

            builder.HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
