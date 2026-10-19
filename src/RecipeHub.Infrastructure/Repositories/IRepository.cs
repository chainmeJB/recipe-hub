using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Query(bool tracking = false);
        ValueTask<T?> GetByIdAsync(object id, CancellationToken ct = default);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
