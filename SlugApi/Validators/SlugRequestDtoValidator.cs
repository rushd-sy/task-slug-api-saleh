using FluentValidation;
using SlugApi.DTOs;

namespace SlugApi.Validators
{
    internal sealed class SlugRequestDtoValidator : AbstractValidator<GenerateSlugRequest>
    {
        public SlugRequestDtoValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("text is requaird")
                .MaximumLength(500)
                .WithMessage("text must be less than 500 character");

            RuleFor(x => x.Separator).Must(c => c == '-' || c == '_')
                .WithMessage("separator must be - or _");

        }
    }
}
