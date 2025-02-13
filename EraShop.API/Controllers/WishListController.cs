using EraShop.API.Contracts.Products;
using EraShop.API.Contracts.WishList;
using EraShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishListController(IWishListService wishListService) : ControllerBase
    {
        private readonly IWishListService _wishListService = wishListService;

        [HttpPost("")]
        public async Task<IActionResult> AddList([FromBody] CreateListRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.AddListAsync(request, cancellationToken);
            return response.IsSuccess ? Ok() 
                : response.ToProblem();
        }

        [HttpPost("{listId}/items")]
        public async Task<IActionResult> AddProductToList([FromRoute] int listId  ,[FromBody] AddProudctToListRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.AddProductToWishListAsync( listId , request, cancellationToken);
            return response.IsSuccess ? Ok()
                : response.ToProblem();
        }

        [HttpGet("{listId}/items")]
        public async Task<IActionResult> GetProductsFromWishList([FromRoute] int listId, CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.GetProductsFromWishListAsync(listId, cancellationToken);
            return response.IsSuccess ? Ok(response.Value)
                : response.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllWishList( CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.GetAllWishListsAsync(cancellationToken);
            return response.IsSuccess ? Ok(response.Value)
                : response.ToProblem();
        }

        [HttpPut("{listId}")]
        public async Task<IActionResult> GetAllWishList([FromRoute] int listId , UpdateWishListRequest request,CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.UpdateWishListAsync(listId,request,cancellationToken);
            return response.IsSuccess ? NoContent()
                : response.ToProblem();
        }

        [HttpDelete("{listId}/items")]
        public async Task<IActionResult> DeleteProductFromWishList([FromRoute] int listId, [FromBody] DeleteProductFromListRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.DeleteProductFromWishListAsync(listId, request, cancellationToken);
            return response.IsSuccess ? NoContent()
                : response.ToProblem();
        }

        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteWishList([FromRoute] int listId, CancellationToken cancellationToken = default)
        {
            var response = await _wishListService.DeleteWishListAsync(listId, cancellationToken);
            return response.IsSuccess ? NoContent()
                : response.ToProblem();
        }
    }
}
