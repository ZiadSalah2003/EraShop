using EraShop.API.Contracts.Files;
using FluentValidation;

namespace EraShop.API.Contracts.Products
{
	public class ProductRequestValidation : AbstractValidator<ProductRequest>
	{
        public ProductRequestValidation()
        {
            RuleFor(x => x.Name)
				.NotEmpty()
				.NotNull()
				.MaximumLength(100);

			RuleFor(x => x.Description)
				.NotEmpty()
				.NotNull()
				.MaximumLength(500);

			RuleFor(x => x.Price)
				.NotEmpty()
				.NotNull()
				.GreaterThan(0);

			RuleFor(x => x.Quantity)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.BrandId)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.CategoryId)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.ImageUrl)
				.SetValidator(new BlockedSignaturesValidator()!)
				.SetValidator(new FileSizeValidator()!);
		}
    }
}
