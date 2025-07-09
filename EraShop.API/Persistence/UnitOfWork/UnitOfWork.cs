using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Persistence;
using EraShop.API.Persistence.Repository;
using System.Collections.Concurrent;

namespace EraShop.API.Persistence.UnitOfWork
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        #region Fields

        private readonly ApplicationDbContext _context = dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();

        #endregion

        #region Implementation of IUnitOfWork
        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public ValueTask DisposeAsync() => _context.DisposeAsync();

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : class
            where TKey : IEquatable<TKey>
        {
            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_context));
        }
        #endregion
    }
} 