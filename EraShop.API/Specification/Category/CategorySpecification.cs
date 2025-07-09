using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.Category
{
    public class CategorySpecification : Specification<EraShop.API.Entities.Category, int>
    {
        public CategorySpecification() : base()
        {
            AddOrderBy(c => c.Name);
            AddIncludes();
        }
        
        public CategorySpecification(Expression<Func<EraShop.API.Entities.Category, bool>>? expression) : base(expression)
        {
            AddOrderBy(c => c.Name);
            AddIncludes();
        }

        public CategorySpecification(int id) : base(c => c.Id == id)
        {
            AddIncludes();
        }

        public CategorySpecification(string name) : base(c => c.Name.ToLower() == name.ToLower())
        {
            AddIncludes();
        }

        public CategorySpecification(bool isActive) : base(c => c.IsDisable == !isActive)
        {
            AddOrderBy(c => c.Name);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
        }
    }
} 