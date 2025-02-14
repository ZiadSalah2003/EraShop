
using FluentValidation;

namespace EraShop.API.Contracts.WishList
{
    public class CreateListRequestValidator : AbstractValidator<CreateListRequest>
    {
        public CreateListRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .MinimumLength(3);
        }
    }
}
