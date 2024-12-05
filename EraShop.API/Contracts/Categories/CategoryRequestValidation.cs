using EraShop.API.Contracts.Authentication;
using FluentValidation;

namespace EraShop.API.Contracts.Categories
{
	public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
	{
		public CategoryRequestValidation()
		{
			RuleFor(x => x.Name)
				.NotNull()
				.NotEmpty()
				.MaximumLength(100);
		}
	}
}
