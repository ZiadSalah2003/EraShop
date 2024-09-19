
using EraShop.API.Abstractions;
using EraShop.API.Authentication;
using EraShop.API.Contracts.Authentication;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Helpers;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace EraShop.API.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
		private readonly IJwtProvider _jwtProvider;
		private readonly int _refreshTokenExpiryDays = 7;
		private readonly ILogger<AuthService> _logger;
		private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<ApplicationUser> userManager,
			   ApplicationDbContext context, IJwtProvider jwtProvider,
			   SignInManager<ApplicationUser> signInManager ,
			   ILogger<AuthService> logger,
			   IEmailSender emailSender ,
              IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _jwtProvider = jwtProvider;
            _signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;	
			_httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
		{
           
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {
                var userRole = await GetUserRole(user, cancellationToken);
                var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRole);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

                return Result.Success(response);
            }

            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentails);
        }

		public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);
			if (userId is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
			if (userRefreshToken is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);

			return Result.Success();
		}

		private static string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

		private async Task<string> GetUserRole(ApplicationUser user, CancellationToken cancellationToken)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			return userRoles.FirstOrDefault()!;
		}

        public async Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
        {
            var emailIsExist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

			if (emailIsExist)
				return Result.Failure(UserErrors.DublicatedEmail);

			var user = request.Adapt<ApplicationUser>();

			var result = await _userManager.CreateAsync(user,request.Password);

			if (result.Succeeded)
			{
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Confirmation Code: {code}", code);
				await SendConfirmationEmail(user, code);
                return Result.Success();

			}


            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

		public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
		{
			if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
				return Result.Failure(UserErrors.InvalidCode);

			if (user.EmailConfirmed)
				return Result.Failure(UserErrors.DuplicatedConfirmation);

			var code = request.Code;

			try
			{
				code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
			}
			catch (FormatException)
			{
				return Result.Failure(UserErrors.InvalidCode);
			}

			var result = await _userManager.ConfirmEmailAsync(user, code);

			if (result.Succeeded)
				return Result.Success();

			var error = result.Errors.First();

			return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

		}

        public async Task<Result> ResendConfirmationEmailAsync(ResendEmailConfirmationRequest request)
		{
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			_logger.LogInformation("Confirmation Code: {code}", code);

           await SendConfirmationEmail(user, code);


            return Result.Success();
        }

		private async Task SendConfirmationEmail(ApplicationUser user , string code)
		{

            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",

                new Dictionary<string, string>
                {
                    { "{{name}}" ,user.FirstName },
                        { "{{action_url}}" ,$"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
                }

                );

            await _emailSender.SendEmailAsync(user.Email!, "✅ EraShop: Email Confirmation", emailBody);
        }

    }
}
