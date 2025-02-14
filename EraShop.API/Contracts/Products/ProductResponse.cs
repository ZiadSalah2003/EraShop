using EraShop.API.Contracts.Brands;
using EraShop.API.Contracts.Categories;

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
		BrandResponse Brand,
		CategoryResponse Category
	);
}
