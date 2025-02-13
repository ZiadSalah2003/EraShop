namespace EraShop.API.Errors
{
    public class WishListErrors
    {
        public static readonly Error ListNameIsExist = 
            new("List.ListNameIsExist", "This List Already Exist", StatusCodes.Status400BadRequest);

        public static readonly Error ListNotFound =
            new("List.ListNotFound", "This List Is not found ", StatusCodes.Status400BadRequest);

        public static readonly Error ProductAlreadyExistInList =
            new("List.ProductAlreadyExistInList", " This Product Already Exist in your list ", StatusCodes.Status400BadRequest);

    }
}
