using EraShop.API.Abstractions.Consts;
using FluentValidation;

namespace EraShop.API.Contracts.Authentication
{
	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress()
				.NotEmpty();

			RuleFor(x => x.Code)
				.NotEmpty();

			RuleFor(x => x.NewPassword)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Passwrod should be at least 8 digits and should contains lower case, nonalphanumeric and uppercase");

		}
	}
}
