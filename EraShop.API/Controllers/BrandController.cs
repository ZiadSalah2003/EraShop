using EraShop.API.Abstractions;
using EraShop.API.Contracts.Brands;
using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController(IBrandService brandService) : ControllerBase
    {
        private readonly IBrandService _brandService = brandService;

        [HttpPost("")]
        public async Task<IActionResult> Add([FromForm] BrandRequest request, CancellationToken cancellationToken)
        {
            var result = await _brandService.AddBrandAsync(request, cancellationToken);


            return result.IsSuccess
                ?Ok()
                : result.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            var result = await _brandService.GetAllBrandAsync(cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }
    }
}
