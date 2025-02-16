namespace EraShop.API.Contracts.Review
{
    public record ReviewResponse
    (
        int ReviewId,
        string Comment,
        int Rating ,
        string FullName
    );
}
