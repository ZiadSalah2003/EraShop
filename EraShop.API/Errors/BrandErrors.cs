using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
    public class BrandErrors
    {
        public static readonly Error BrandNotFound = new("Brand.NotFound", "No Brand was found with the given id", StatusCodes.Status404NotFound);
        public static readonly Error DublicatedName = new("Brand.DublicatedName", "Another Brand with the same name is already exists", StatusCodes.Status409Conflict);
    }
}
