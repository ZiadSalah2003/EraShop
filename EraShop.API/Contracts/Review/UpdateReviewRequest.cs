namespace EraShop.API.Contracts.Review
{
    public record UpdateReviewRequest
    (
         string Comment,
        int Rating
    );
}
