using Microsoft.Extensions.DependencyInjection;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Infrastructure
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddRecipeHubData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RecipeHubContext>(options =>
            {
                // TODO: add actual DB provider and connection string configuration here
            });
            
            return services;
        }

        public static IServiceCollection AddRecipeHubRepositories(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
