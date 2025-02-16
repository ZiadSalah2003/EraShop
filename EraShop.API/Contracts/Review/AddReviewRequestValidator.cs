using FluentValidation;

namespace EraShop.API.Contracts.Review
{
    public class AddReviewRequestValidator : AbstractValidator<AddReviewRequest>
    {
        public AddReviewRequestValidator()
        {
            RuleFor(x => x.Comment)
                .NotNull()
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Rating)
                .NotEmpty()
                .ExclusiveBetween(0, 6)
                .WithMessage("Rating Must Be Between 1 and 5 Including Them");
        }
    }
}
