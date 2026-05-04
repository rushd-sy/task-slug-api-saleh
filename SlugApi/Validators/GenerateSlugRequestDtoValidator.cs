using FluentValidation;
using SlugApi.DTOs;

namespace SlugApi.Validators
{
    internal sealed class GenerateSlugRequestDtoValidator : AbstractValidator<GenerateSlugRequest>
    {
        public GenerateSlugRequestDtoValidator()
        {
            RuleFor(x => x.Text).Must(text => !string.IsNullOrWhiteSpace(text))
                .WithMessage("Text is required.")
                .MaximumLength(500)
                .WithMessage("Text must be less than 500 characters");

            RuleFor(x => x.Separator).Must(c => !c.HasValue || c == '-' || c == '_')
                .WithMessage("separator must be - or _");

        }
    }
}
