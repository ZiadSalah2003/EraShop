using EraShop.API.Abstractions;
using EraShop.API.Contracts.Authentication;

namespace EraShop.API.Services
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
		Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
		Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
		Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
		Task<Result> ResendConfirmationEmailAsync(ResendEmailConfirmationRequest request);
		Task<Result> SendResetPasswordCodeAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);


	}
}
