using EraShop.API.Entities;
using System.Linq.Expressions;

namespace EraShop.API.Specification.WishList
{
    public class ListItemSpecification : Specification<ListItem, int>
    {
        public ListItemSpecification() : base()
        {
            AddIncludes();
        }
        
        public ListItemSpecification(Expression<Func<ListItem, bool>>? expression) : base(expression)
        {
            AddIncludes();
        }

        public ListItemSpecification(int listId, int productId) : base(li => li.ListId == listId && li.ProductId == productId)
        {
            AddIncludes();
        }

        public ListItemSpecification(int listId) : base(li => li.ListId == listId)
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            Includes.Add(li => li.Product);
            Includes.Add(li => li.List);
        }
    }
} 