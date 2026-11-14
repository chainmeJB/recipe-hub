using FluentValidation;
using RecipeHub.Api.Request_Models;

namespace RecipeHub.Api.Validators
{
    public class RecipeIngredientValidator : AbstractValidator<RecipeIngredientRequestModel>
    {
        public RecipeIngredientValidator()
        {
            RuleFor(x => x.IngredientId)
                .GreaterThan(0).WithMessage("IngredientId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required.")
                .MaximumLength(30);

            RuleFor(x => x.Note)
                .MaximumLength(200)
                .When(x => x.Note is not null);
        }
    }
}
