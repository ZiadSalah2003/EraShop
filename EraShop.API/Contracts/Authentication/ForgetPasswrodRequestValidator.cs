using FluentValidation;

namespace EraShop.API.Contracts.Authentication
{
	public class ForgetPasswrodRequestValidator : AbstractValidator<ForgetPasswrodRequest>
	{
		public ForgetPasswrodRequestValidator()
		{
			RuleFor(x => x.Email)
				.EmailAddress()
				.NotEmpty();
		}
	}
}
