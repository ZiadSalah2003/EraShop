namespace EraShop.API.Contracts.Baskets
{
	public record CustomerBasketResponse
	(
		string Id,
		IEnumerable<BasketItemResponse> Items,
		string? PaymentIntentId,
		string? ClientSecret,
		int? DeliveryMethodId,
		decimal ShippingPrice
	);
}
