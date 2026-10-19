using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(i => i.Name).IsUnique();
        }
    }
}
