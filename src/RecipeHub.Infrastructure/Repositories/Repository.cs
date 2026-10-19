using Microsoft.EntityFrameworkCore;

namespace RecipeHub.Infrastructure.Repositories
{
    public class Repository<T>(RecipeHubContext context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set = context.Set<T>();

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public ValueTask<T?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            return _set.FindAsync([id], ct);
        }

        public IQueryable<T> Query(bool tracking = false)
        {
            return tracking ? _set : _set.AsNoTracking();
        }

        public void Remove(T entity)
        {
            _set.Remove(entity);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return context.SaveChangesAsync(ct);
        }

        public void Update(T entity)
        {
            var entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _set.Attach(entity);
            entry.State = EntityState.Modified;
        }
    }
}
