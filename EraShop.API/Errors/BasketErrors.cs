using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class BasketErrors
	{
		public static readonly Error BasketNotFound = new("Basket.NotFound", "No Basket was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error BasketUpdateFailed = new("Basket.UpdateFailed", "Failed to update the Basket", StatusCodes.Status400BadRequest);
	}
}
