using FluentValidation;
using RecipeHub.Api.Request_Models;

namespace RecipeHub.Api.Validators
{
    public class RecipeValidator : AbstractValidator<RecipeRequestModel>
    {
        public RecipeValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 150);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => x.Description is not null);

            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("Instructions are required.")
                .MinimumLength(10);

            RuleFor(x => x.Servings)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.CookTimeMinutes)
                .InclusiveBetween(1, 10080);

            RuleFor(x => x.RecipeIngredients)
                .NotEmpty().WithMessage("Recipe must contain at least one ingredient.");

            RuleFor(x => x.Images)
                .Must(images => images == null || images.Count(i => i.IsPrimary) <= 1)
                .WithMessage("Only one image can be marked as primary.");
        }
    }
}
