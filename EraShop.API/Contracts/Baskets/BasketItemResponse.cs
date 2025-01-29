namespace EraShop.API.Contracts.Baskets
{
	public record BasketItemResponse
	(
		int Id,
		string ProductName,
		string PictureUrl,
		decimal Price,
		int Quantity,
		string Brand,
		string Category
	);
}
