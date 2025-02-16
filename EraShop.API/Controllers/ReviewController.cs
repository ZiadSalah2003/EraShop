using EraShop.API.Contracts.Products;
using EraShop.API.Contracts.Review;
using EraShop.API.Contracts.WishList;
using EraShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/products/{productId}/reviews")]
    [ApiController]
    [Authorize]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpPost("")]
        public async Task<IActionResult> AddReview([FromRoute] int productId, [FromBody] AddReviewRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _reviewService.AddReviewAsync(productId,request, cancellationToken);
            return response.IsSuccess 
                ? Ok()
                : response.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllReviews([FromRoute] int productId, CancellationToken cancellationToken = default)
        {
            var result = await _reviewService.GetAllReviewsAsync(productId,cancellationToken);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPut("Update/{ReviewId}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int productId, [FromRoute] int ReviewId,UpdateReviewRequest request , CancellationToken cancellationToken = default)
        {
            var result = await _reviewService.UpdateReviewAsync(productId, ReviewId, request, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("{ReviewId}")]
        public async Task<IActionResult> GetAllReviews([FromRoute] int productId, [FromRoute] int ReviewId, CancellationToken cancellationToken = default)
        {
            var result = await _reviewService.ToggleStatusAsync(productId, ReviewId, cancellationToken);
            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }


    }
}
