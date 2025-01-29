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

			RuleFor(x => x.Items)
				.NotEmpty()
				.NotNull();
		}
	}
}
