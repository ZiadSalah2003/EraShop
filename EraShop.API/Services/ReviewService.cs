using EraShop.API.Contracts.Infrastructure;
using EraShop.API.Contracts.Review;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Specification.Product;
using EraShop.API.Specification.Review;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EraShop.API.Services
{
    public class ReviewService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddReviewAsync(int productId, AddReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
            var reviewRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Review, int>();
            
            var productSpec = new ProductSpecification(productId);
            var product = await productRepository.GetWithSpecAsync(productSpec);

            if (product is null || product.IsDisable)
                return Result.Failure(ProductErrors.ProductNotFound);

            var existingReviewSpec = new ReviewSpecification(r => r.ProductId == productId && r.UserId == userId);
            var existingReview = await reviewRepository.GetWithSpecAsync(existingReviewSpec);
            if (existingReview != null)
                return Result.Failure(ReviewErrors.ReviewAlreadyExists);

            var review = new EraShop.API.Entities.Review
            {
                ProductId = productId,
                UserId = userId!,
                Comment = request.Comment,
                Rating = request.Rating,
                IsDisable = false
            };

            await reviewRepository.AddAsync(review, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<ReviewResponse>>> GetAllReviewsAsync(int productId, CancellationToken cancellationToken)
        {
            var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
            var reviewRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Review, int>();
            
            var productSpec = new ProductSpecification(productId);
            var product = await productRepository.GetWithSpecAsync(productSpec);

            if (product is null || product.IsDisable)
                return Result.Failure<IEnumerable<ReviewResponse>>(ProductErrors.ProductNotFound);

            var reviewsSpec = new ReviewSpecification(r => r.ProductId == productId && !r.IsDisable);
            var reviews = await reviewRepository.GetAllWithSpecAsync(reviewsSpec);

            var reviewResponses = reviews.Select(x => new ReviewResponse(
                x.Id,
                x.Comment,
                x.Rating ?? 0,
                $"{x.User!.FirstName} {x.User.LastName}"
            ));

            return Result.Success<IEnumerable<ReviewResponse>>(reviewResponses);
        }
        public async Task<Result> UpdateReviewAsync(int productId, int ReviewId, UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
            var reviewRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Review, int>();
            
            var productSpec = new ProductSpecification(productId);
            var product = await productRepository.GetWithSpecAsync(productSpec);

            if (product is null || product.IsDisable)
                return Result.Failure(ProductErrors.ProductNotFound);

            var reviewSpec = new ReviewSpecification(r => r.Id == ReviewId && r.UserId == userId && r.ProductId == productId && !r.IsDisable);
            var review = await reviewRepository.GetWithSpecAsync(reviewSpec);

            if (review is null)
                return Result.Failure(ReviewErrors.ReviewNotFound);

            review.Comment = request.Comment;
            review.Rating = request.Rating;

            reviewRepository.Update(review);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result> ToggleStatusAsync(int productId, int ReviewId, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Product, int>();
            var reviewRepository = _unitOfWork.GetRepository<EraShop.API.Entities.Review, int>();
            
            var productSpec = new ProductSpecification(productId);
            var product = await productRepository.GetWithSpecAsync(productSpec);

            if (product is null || product.IsDisable)
                return Result.Failure(ProductErrors.ProductNotFound);

            var reviewSpec = new ReviewSpecification(r => r.Id == ReviewId && r.UserId == userId && r.ProductId == productId);
            var review = await reviewRepository.GetWithSpecAsync(reviewSpec);

            if(review is null)
                return Result.Failure(ReviewErrors.ReviewNotFound);

            review.IsDisable = !review.IsDisable;
            reviewRepository.Update(review);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
    }
}
