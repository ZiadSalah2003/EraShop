using FluentValidation;

namespace EraShop.API.Contracts.WishList
{
    public class AddProudctToListRequestValidator : AbstractValidator<AddProudctToListRequest>
    {
        public AddProudctToListRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .NotNull();
        }
    }
}
