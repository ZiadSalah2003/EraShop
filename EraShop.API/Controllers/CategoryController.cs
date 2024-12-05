using EraShop.API.Abstractions;
using EraShop.API.Contracts.Categories;
using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
		[HttpGet("")]
		public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
		{
			var response = await _categoryService.GetAllAdync(cancellationToken);
			return Ok(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory([FromRoute] int id, CancellationToken cancellationToken)
		{
			var response = await _categoryService.GetByIdAsync(id, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPost("")]
		public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
		{
			var response = await _categoryService.AddAsync(request);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
		{
			var response = await _categoryService.UpdateAsync(id, request, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
		[HttpPut("{id}/toggle-status")]
		public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
		{
			var response = await _categoryService.ToggleStatus(id, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
	}
}
