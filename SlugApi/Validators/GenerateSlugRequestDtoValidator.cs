using FluentValidation;
using SlugApi.DTOs;

namespace SlugApi.Validators
{
    internal sealed class GenerateSlugRequestDtoValidator : AbstractValidator<GenerateSlugRequest>
    {
        public GenerateSlugRequestDtoValidator()
        {
            RuleFor(x => x.Text).NotNull().NotEmpty()
                .WithMessage("Text is required.")
                .MaximumLength(500)
                .WithMessage("Text must be 500 or fewer characters");

            RuleFor(x => x.Separator).Must(c => !c.HasValue || c == '-' || c == '_')
                .WithMessage("Separator must be - or _");

        }
    }
}
