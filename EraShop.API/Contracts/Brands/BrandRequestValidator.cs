using FluentValidation;

namespace EraShop.API.Contracts.Brands
{
    public class BrandRequestValidator : AbstractValidator<BrandRequest>
    {
        public BrandRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100);

            RuleFor(x => x.Image)
                  .NotEmpty()
                  .NotNull();
        }
    }
}
