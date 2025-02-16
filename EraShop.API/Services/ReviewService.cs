using EraShop.API.Contracts.Review;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EraShop.API.Services
{
    public class ReviewService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IReviewService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> AddReviewAsync(int productId, AddReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId && !x.IsDisable, cancellationToken);

            if (!productExists)
                return Result.Failure(ProductErrors.ProductNotFound);

            var existingReview = await _context.Reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId, cancellationToken);
            if (existingReview != null)
                return Result.Failure(ReviewErrors.ReviewAlreadyExists);

            var review = new Review
            {
                ProductId = productId,
                UserId = userId!,
                Comment = request.Comment,
                Rating = request.Rating,
                IsDisable = false
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result<IEnumerable<ReviewResponse>>> GetAllReviewsAsync(int productId, CancellationToken cancellationToken)
        {
            var productExists = await _context.Products.AnyAsync(x => x.Id == productId && !x.IsDisable, cancellationToken);

            if (!productExists)
                return Result.Failure<IEnumerable<ReviewResponse>>(ProductErrors.ProductNotFound);

            var reviews = await _context.Reviews
                                            .Where(r => r.ProductId == productId && !r.IsDisable)
                                            .Select(x => new ReviewResponse(
                                                x.Id,
                                                x.Comment,
                                                x.Rating ?? 0,
                                                $"{x.User!.FirstName} {x.User.LastName}"
                                            ))
                                            .ToListAsync();


            return Result.Success<IEnumerable<ReviewResponse>>(reviews);
        }
        public async Task<Result> UpdateReviewAsync(int productId, int ReviewId, UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId && !x.IsDisable, cancellationToken);

            if (!productExists)
                return Result.Failure(ProductErrors.ProductNotFound);

            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == ReviewId && x.UserId == userId && x.ProductId == productId && !x.IsDisable);

            if (review is null)
                return Result.Failure(ReviewErrors.ReviewNotFound);

               review.Comment = request.Comment;
               review.Rating = request.Rating;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
        public async Task<Result> ToggleStatusAsync(int productId, int ReviewId, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId && !x.IsDisable, cancellationToken);

            if (!productExists)
                return Result.Failure(ProductErrors.ProductNotFound);

            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == ReviewId && x.UserId == userId && x.ProductId == productId);

            if(review is null)
                return Result.Failure(ReviewErrors.ReviewNotFound);

            review.IsDisable = !review.IsDisable;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }


    }
}
