using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Common;
using EraShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Admin)]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetReportAsync()
        {
            var response = await _reportService.GetDailyReport();
            return Ok(response.Value);
        }
    }
}
