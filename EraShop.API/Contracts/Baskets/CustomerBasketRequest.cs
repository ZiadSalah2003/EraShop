namespace EraShop.API.Contracts.Baskets
{
	public record CustomerBasketRequest
	(
		string Id,
		IEnumerable<BasketItemResponse> Items
	);
}
