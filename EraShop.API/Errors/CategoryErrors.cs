using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class CategoryErrors
	{
		public static readonly Error CategoryNotFound = new("Category.NotFound", "No Category was found with the given id", StatusCodes.Status404NotFound);

		public static readonly Error DublicatedName = new("Category.DublicatedName", "Another Category with the same name is already exists", StatusCodes.Status409Conflict);
	}
}
