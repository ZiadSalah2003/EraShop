namespace EraShop.API.Contracts.Orders
{
	public record OrderCreateRequest
	(
		string BasketId,
		int DeliveryMethodId,
		AdressReponse ShipToAddress
	);
}
