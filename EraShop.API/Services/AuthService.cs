
using EraShop.API.Authentication;
using EraShop.API.Contracts.Authentication;
using EraShop.API.Entities;
using EraShop.API.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EraShop.API.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IJwtProvider _jwtProvider;

		public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IJwtProvider jwtProvider)
		{
			_userManager = userManager;
			_context = context;
			_jwtProvider = jwtProvider;

		}

		public async Task<AuthResponse> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return null;

			//if (!user.IsDisabled)
			//	return null;
			
			var checkPassword = await _userManager.CheckPasswordAsync(user, password);
			if (!checkPassword)
				return null;

			var userRole = await GetUserRole(user, cancellationToken);
			var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRole);

			await _userManager.UpdateAsync(user);
			return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
		}

		private async Task<string> GetUserRole(ApplicationUser user, CancellationToken cancellationToken)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			return userRoles.FirstOrDefault()!;
		}
	}
}
