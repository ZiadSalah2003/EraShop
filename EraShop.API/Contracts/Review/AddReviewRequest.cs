namespace EraShop.API.Contracts.Review
{
    public record AddReviewRequest
    (
        string Comment,
        int Rating
    );
}
