namespace EraShop.API.Contracts.Products
{
	public record ProductRequest
	(
		string Name,
		string Description,
		decimal Price,
		IFormFile? ImageUrl,
		int Quantity,
		int BrandId,
		int CategoryId
	);
}
