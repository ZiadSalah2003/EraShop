namespace EraShop.API.Contracts.Orders
{
	public record OrderItemResponse
	(
		int Id,
		int ProductId,
		string ProductName,
		string PictureUrl,
		decimal Price,
		int Quantity
	);
}
