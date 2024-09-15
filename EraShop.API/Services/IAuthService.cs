using EraShop.API.Contracts.Authentication;

namespace EraShop.API.Services
{
	public interface IAuthService
	{
		Task<AuthResponse> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
	}
}
