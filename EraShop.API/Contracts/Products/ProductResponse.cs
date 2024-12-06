namespace EraShop.API.Contracts.Products
{
	public record ProductResponse
	(
		int Id,
		string Name,
		string Description,
		decimal Price,
		string ImageUrl,
		int Quantity,
		bool IsDisable,
		string Brand,
		string Category
	);
}
