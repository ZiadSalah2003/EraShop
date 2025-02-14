using System.Security.Claims;

namespace EraShop.API.Extensions
{
	public static class UserExtensions
	{
		public static string? GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.NameIdentifier);
		}
		public static string? GetUserEmail(this ClaimsPrincipal user)
		{
			return user.FindFirstValue(ClaimTypes.Email);
		}
	}
}
