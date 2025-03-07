using EraShop.API.Contracts.Brands;
using FluentValidation;

namespace EraShop.API.Contracts.Baskets
{
	public class CustomerBasketRequestValidation : AbstractValidator<CustomerBasketRequest>
	{
		public CustomerBasketRequestValidation()
		{
			RuleFor(x => x.Id)
				.NotEmpty()
				.NotNull();

			RuleForEach(x => x.Items)
				.NotEmpty()
				.NotNull()
				.SetInheritanceValidator(b =>
				b.Add(new BasketItemResponseValidator()));
		}
	}
}
