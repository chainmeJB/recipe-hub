using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RecipeHub.Infrastructure.Entities;

namespace RecipeHub.Infrastructure
{
    public class RecipeHubContext(DbContextOptions<RecipeHubContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeHubContext).Assembly);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<Recipe>()
                         .Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.UpdatedAt = now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
