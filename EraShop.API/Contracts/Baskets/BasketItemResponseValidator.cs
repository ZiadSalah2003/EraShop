using FluentValidation;

namespace EraShop.API.Contracts.Baskets
{
    public class BasketItemResponseValidator : AbstractValidator<BasketItemResponse>
    {
        public BasketItemResponseValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ProductName)
                .NotNull()
               .NotEmpty();

            RuleFor(x => x.PictureUrl)
                .NotNull()
               .NotEmpty();

            RuleFor(x => x.Price)
               .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
               .NotEmpty();

            RuleFor(x => x.Brand)
                .NotNull()
               .NotEmpty();

            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty();
        }
    }
}
