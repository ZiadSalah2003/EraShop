using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class UserErrors
	{
		public static readonly Error InvalidCredentails = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);
	}
}
