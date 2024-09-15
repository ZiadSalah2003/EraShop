using EraShop.API.Abstractions;
using EraShop.API.Contracts.Authentication;

namespace EraShop.API.Services
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
		Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

	}
}
