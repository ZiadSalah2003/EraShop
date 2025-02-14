namespace EraShop.API.Contracts.Orders
{
	public record OrderResponse
	(
		int Id,
		string BuyerEmail,
		DateTime OrderDate,
		string Status,
		AdressReponse ShippingAddress,
		int? DeliveryMethodId,
		string? DeliveryMethod,
		ICollection<OrderItemResponse> Items,
		decimal Subtotal,
		decimal Total,
		string PaymentIntentId
	);
}
