using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.Brand
{
    public class BrandSpecification : Specification<EraShop.API.Entities.Brand, int>
    {
        public BrandSpecification() : base()
        {
            AddOrderBy(b => b.Name);
            AddIncludes();
        }
        
        public BrandSpecification(Expression<Func<EraShop.API.Entities.Brand, bool>>? expression) : base(expression)
        {
            AddOrderBy(b => b.Name);
            AddIncludes();
        }

        public BrandSpecification(int id) : base(b => b.Id == id)
        {
            AddIncludes();
        }

        public BrandSpecification(string name) : base(b => b.Name.ToLower() == name.ToLower())
        {
            AddIncludes();
        }

        public BrandSpecification(bool isActive) : base(b => b.IsDisable == !isActive)
        {
            AddOrderBy(b => b.Name);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
        }
    }
} 