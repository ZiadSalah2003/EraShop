using FluentValidation;

namespace EraShop.API.Contracts.WishList
{
    public class UpdateWishListRequestValidator : AbstractValidator<UpdateWishListRequest>
    {
        public UpdateWishListRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
