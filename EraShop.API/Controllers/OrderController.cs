using Azure;
using EraShop.API.Abstractions;
using EraShop.API.Contracts.Orders;
using EraShop.API.Extensions;
using EraShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}
		[HttpPost("")]
		public async Task<IActionResult> CreateOrder(OrderCreateRequest request)
		{
			var response = await _orderService.CreateOrderAsync(User.GetUserEmail()!, request);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpGet("")]
		public async Task<IActionResult> GetOrders()
		{
			var response = await _orderService.GetOrdersForUserAsync(User.GetUserEmail()!);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpGet("{orderId}")]
		public async Task<IActionResult> GetOrderById(int orderId)
		{
			var response = await _orderService.GetOrderByIdAsync(User.GetUserEmail()!, orderId);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpGet("delivery-methods")]
		public async Task<IActionResult> GetDeliveryMethods()
		{
			var response = await _orderService.GetDeliveryMethodsAsync();
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		//[HttpPut("{id}/status")]
		//public async Task<ActionResult<OrderResponse>> UpdateOrderStatus(string id, OrderStatusUpdateRequest request)
		//{
		//	var order = await _orderService.UpdateOrderStatusAsync(id, request);
		//	return Ok(order);
		//}
	}
}
