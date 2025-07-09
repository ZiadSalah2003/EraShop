using EraShop.API.Contracts;
using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Specification;
using EraShop.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EraShop.API.Persistence.Repository
{
    internal class GenericRepository<TEntity, TKey>(ApplicationDbContext _context) : IGenericRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        #region Fields

        #endregion
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false, CancellationToken cancellationToken = default)
        {
            return withTracking ?
                await _context.Set<TEntity>().ToListAsync(cancellationToken) :
                await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false)
        => await ApplySpecification(specifications).ToListAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false)
        => await ApplySpecification(specifications).CountAsync();

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> specifications)
        => await ApplySpecification(specifications).FirstOrDefaultAsync();

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> specifications)
          => SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specifications);
    }
} 