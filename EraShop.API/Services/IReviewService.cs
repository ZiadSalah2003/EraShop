using EraShop.API.Contracts.Review;

namespace EraShop.API.Services
{
    public interface IReviewService
    {
        Task<Result> AddReviewAsync(int productId , AddReviewRequest request , CancellationToken cancellationToken);
        Task<Result<IEnumerable<ReviewResponse>>> GetAllReviewsAsync(int productId , CancellationToken cancellationToken);
        Task<Result>ToggleStatusAsync(int productId ,int ReviewId, CancellationToken cancellationToken);
        Task<Result> UpdateReviewAsync(int productId, int ReviewId, UpdateReviewRequest request, CancellationToken cancellationToken);

    }
}
