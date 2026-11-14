using FluentValidation;
using RecipeHub.Api.Request_Models;

namespace RecipeHub.Api.Validators
{
    public class ImageValidator : AbstractValidator<ImageRequestModel>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(2048)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                             (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                .WithMessage("Image URL must be a valid HTTP or HTTPS URL.");

            RuleFor(x => x.Position)
                .GreaterThanOrEqualTo(0).WithMessage("Position cannot be negative.");
        }
    }
}
