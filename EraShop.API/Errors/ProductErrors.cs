using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class ProductErrors
	{
		public static readonly Error ProductNotFound = new("Product.NotFound", "No Product was found with the given id", StatusCodes.Status404NotFound);

		public static readonly Error DublicatedName = new("Product.DublicatedName", "Another Product with the same name is already exists", StatusCodes.Status409Conflict);
		
		public static readonly Error ProductImageExcced1M = new("Product.ImageExcced1M ", "Product File size shouldn't exceed 1MB", StatusCodes.Status400BadRequest);

	}
}
