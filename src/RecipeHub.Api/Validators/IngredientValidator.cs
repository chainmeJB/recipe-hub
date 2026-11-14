using FluentValidation;
using RecipeHub.Api.Request_Models;

namespace RecipeHub.Api.Validators
{
    public class IngredientValidator : AbstractValidator<IngredientRequestModel>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ingredient name is required.")
                .Length(2, 64).WithMessage("Ingredient name must be between 2 and 64 characters.");
        }
    }
}
