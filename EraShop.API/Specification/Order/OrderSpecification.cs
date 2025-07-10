using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.Order
{
    public class OrderSpecification : Specification<Entities.Order, int>
    {
        public OrderSpecification() : base()
        {
            AddOrderByDesc(o => o.OrderDate);
            AddIncludes();
        }
        
        public OrderSpecification(Expression<Func<Entities.Order, bool>>? expression) : base(expression)
        {
            AddOrderByDesc(o => o.OrderDate);
            AddIncludes();
        }

        public OrderSpecification(int id) : base(o => o.Id == id)
        {
            AddIncludes();
        }

        public OrderSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            AddOrderByDesc(o => o.OrderDate);
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }
    }
} 