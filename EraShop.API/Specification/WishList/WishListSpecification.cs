using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.WishList
{
    public class WishListSpecification : Specification<List, int>
    {
        public WishListSpecification() : base()
        {
            AddOrderBy(l => l.Name);
            AddIncludes();
        }
        
        public WishListSpecification(Expression<Func<List, bool>>? expression) : base(expression)
        {
            AddOrderBy(l => l.Name);
            AddIncludes();
        }

        public WishListSpecification(int id) : base(l => l.Id == id)
        {
            AddIncludes();
        }

        public WishListSpecification(string userId) : base(l => l.UserId == userId)
        {
            AddOrderBy(l => l.Name);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(l => l.Items);
            Includes.Add(l => l.User);
        }
    }
} 