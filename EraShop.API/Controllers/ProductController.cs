using EraShop.API.Abstractions;
using EraShop.API.Contracts.Products;
using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		[HttpGet("")]
		public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var response = await _productService.GetAllAdync(cancellationToken);
			return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
		{
			var response = await _productService.GetByIdAsync(id, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPost("")]
		public async Task<IActionResult> AddAsync([FromForm] ProductRequest request, CancellationToken cancellationToken = default)
		{
			var response = await _productService.AddAsync(request, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ProductRequest request, CancellationToken cancellationToken = default)
		{
			var response = await _productService.UpdateAsync(id, request, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
		[HttpPut("{id}/toggle-status")]
		public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken = default)
		{
			var response = await _productService.ToggleStatus(id, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
	}
}
