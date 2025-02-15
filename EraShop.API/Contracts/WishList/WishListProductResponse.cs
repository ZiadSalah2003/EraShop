using EraShop.API.Contracts.Brands;
using EraShop.API.Contracts.Categories;

namespace EraShop.API.Contracts.WishList
{
	public record WishListProductResponse
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
