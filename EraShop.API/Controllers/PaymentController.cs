using EraShop.API.Abstractions;
using EraShop.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}
		[HttpPost("{basketId}")]
		public async Task<IActionResult> CreateOrUpdatePaymentIntent(string basketId)
		{
			var response = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			await _paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);
			return Ok();
		}
	}
}
