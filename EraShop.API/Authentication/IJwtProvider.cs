using EraShop.API.Entities;

namespace EraShop.API.Authentication
{
	public interface IJwtProvider
	{
		(string token, int expireTime) GenerateToken(ApplicationUser user, string role);
		string? ValidateToken(string token);
	}
}
