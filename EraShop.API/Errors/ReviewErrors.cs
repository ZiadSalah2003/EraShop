using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
    public static class ReviewErrors
    {
        public static readonly Error ReviewAlreadyExists =
                 new("Review.ReviewAlreadyExists", "You have already submitted a review for this product", StatusCodes.Status400BadRequest);

        public static readonly Error ReviewNotFound =
               new("Review.ReviewNotFound", "Review Is not Found", StatusCodes.Status400BadRequest);

    }
}
