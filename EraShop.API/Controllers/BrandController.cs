using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Brands;
using EraShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Admin)]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll([FromRoute] int id ,CancellationToken cancellationToken)
        {
            var result = await _brandService.GetBrandByIdAsync(id,cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
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

        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> Toggle([FromRoute] int id ,CancellationToken cancellationToken)
        {
            var result = await _brandService.DeleteBrandAsync(id,cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] BrandRequest request, CancellationToken cancellationToken)
        {
            var result = await _brandService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }


    }
}
