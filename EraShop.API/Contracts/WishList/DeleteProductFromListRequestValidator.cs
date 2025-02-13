using FluentValidation;

namespace EraShop.API.Contracts.WishList
{
    public class DeleteProductFromListRequestValidator : AbstractValidator<DeleteProductFromListRequest>
    {
        public DeleteProductFromListRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .NotNull();
        }
    }
}
