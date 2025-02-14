using FluentValidation;

namespace EraShop.API.Contracts.Orders
{
	public class OrderCreateRequestValidation : AbstractValidator<OrderCreateRequest>
	{
		public OrderCreateRequestValidation()
		{
			RuleFor(x => x.BasketId)
				.NotEmpty();

			RuleFor(x => x.DeliveryMethodId)
				.GreaterThan(0)
				.NotEmpty();
		}
	}
}
