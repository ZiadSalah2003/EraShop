namespace EraShop.API.Contracts.Infrastructure
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class
            where TKey : IEquatable<TKey>;
        Task<int> CompleteAsync();
    }
} 