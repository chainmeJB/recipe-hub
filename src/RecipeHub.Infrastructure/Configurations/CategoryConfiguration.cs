using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeHub.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
