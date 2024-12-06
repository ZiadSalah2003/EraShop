using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class BrandErrors
	{
		public static readonly Error BrandNotFound = new("Brand.NotFound", "No Brand was found with the given id", StatusCodes.Status404NotFound);

		public static readonly Error BrandImageExcced1M = new("Brand.ImageExcced1M ", "Brand File size shouldn't exceed 1MB", StatusCodes.Status400BadRequest);
	}
}
