using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.Review
{
    public class ReviewSpecification : Specification<Entities.Review, int>
    {
        public ReviewSpecification() : base()
        {
            AddOrderBy(r => r.Id);
            AddIncludes();
        }
        
        public ReviewSpecification(Expression<Func<Entities.Review, bool>>? expression) : base(expression)
        {
            AddOrderBy(r => r.Id);
            AddIncludes();
        }

        public ReviewSpecification(int id) : base(r => r.Id == id)
        {
            AddIncludes();
        }

        public ReviewSpecification(string userId) : base(r => r.UserId == userId)
        {
            AddOrderBy(r => r.Id);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(r => r.Product);
            Includes.Add(r => r.User);
        }
    }
} 