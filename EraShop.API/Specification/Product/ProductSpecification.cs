using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.Product
{
    public class ProductSpecification : Specification<EraShop.API.Entities.Product, int>
    {
        public ProductSpecification() : base()
        {
            AddOrderBy(p => p.Name);
            AddIncludes();
        }
        
        public ProductSpecification(Expression<Func<EraShop.API.Entities.Product, bool>>? expression) : base(expression)
        {
            AddOrderBy(p => p.Name);
            AddIncludes();
        }

        public ProductSpecification(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        public ProductSpecification(string name) : base(p => p.Name.ToLower().Contains(name.ToLower()))
        {
            AddOrderBy(p => p.Name);
            AddIncludes();
        }

        public ProductSpecification(bool isActive) : base(p => p.IsDisable == !isActive)
        {
            AddOrderBy(p => p.Name);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(p => p.Category);
            Includes.Add(p => p.Brand);
        }
    }
} 