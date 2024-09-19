using FluentValidation;

namespace EraShop.API.Contracts.Authentication
{
    public class ResendEmailConfirmationRequestValidator : AbstractValidator<ResendEmailConfirmationRequest>
    {
        public ResendEmailConfirmationRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
