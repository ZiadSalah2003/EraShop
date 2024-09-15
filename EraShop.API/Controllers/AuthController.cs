using EraShop.API.Contracts.Authentication;
using EraShop.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EraShop.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
		}

        [HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request,CancellationToken cancellationToken)
		{
			var response = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
			if (response is null)
				return BadRequest();
			return Ok(response);
		}
	}
}
