using EraShop.API.Abstractions.Consts;
using FluentValidation;

namespace EraShop.API.Contracts.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {


            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains LowerCase, NonAlphanumeric and UpperCase")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New Password can't be same as the current password");


        }
    }
}
