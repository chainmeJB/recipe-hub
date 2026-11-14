using FluentValidation;
using RecipeHub.Api.Request_Models;
namespace RecipeHub.Api.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryRequestModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(2, 50);
        }
    }
}
