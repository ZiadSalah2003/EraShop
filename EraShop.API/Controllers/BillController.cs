using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
		private readonly IBillService _billService;
		public BillController(IBillService billService)
		{
			_billService = billService;
		}
		[HttpGet("")]
		public async Task<IActionResult> GetAllBill()
		{
			var result = await _billService.GetAllBillsAsync();
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
		}
	}
}
