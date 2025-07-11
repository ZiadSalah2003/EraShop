using System.Linq.Expressions;

namespace EraShop.API.Contracts
{
    public interface ISpecification<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public bool IsGettingTopQuery { get; set; }
    }
} 