using EraShop.API.Abstractions;
using EraShop.API.Contracts.Baskets;
using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IBasketService _basketService;
		public BasketController(IBasketService basketService)
		{
			_basketService = basketService;
		}

		[HttpGet]
		public async Task<IActionResult> GetBasket(string id)
		{
			var basket = await _basketService.GetCustomerBasketAsync(id);
			return basket.IsSuccess ? Ok(basket.Value) : basket.ToProblem();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateBasket(CustomerBasketRequest request)
		{
			var basket = await _basketService.UpdateCustomerBasketAsync(request);
			return basket.IsSuccess ? Ok(basket.Value) : basket.ToProblem();
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteBasket(string id)
		{
			var basket = await _basketService.DeleteCustomerBasketAsync(id);
			return basket.IsSuccess ? Ok() : basket.ToProblem();
		}
	}
}
