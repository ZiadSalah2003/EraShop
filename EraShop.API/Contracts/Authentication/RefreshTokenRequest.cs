namespace EraShop.API.Contracts.Authentication
{
	public record RefreshTokenRequest
	(
		string Token,
		string RefreshToken
	);
}
