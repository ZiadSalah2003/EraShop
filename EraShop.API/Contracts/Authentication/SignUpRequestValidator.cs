using EraShop.API.Abstractions.Consts;
using FluentValidation;

namespace EraShop.API.Contracts.Authentication
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
               .NotEmpty()
               .Matches(RegexPatterns.Password)
               .WithMessage("Password should be at least 8 digits and should contains LowerCase, NonAlphanumeric and UpperCase");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 20);

            RuleFor(x => x.LastName)
               .NotEmpty()
               .Length(3, 20);
        }
    }
}
