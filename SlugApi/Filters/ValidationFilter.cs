using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SlugApi.Filters
{
    public class ValidationFilter<T> : IActionFilter
    {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator) =>
            _validator = validator;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var arg = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (arg is not null)
            {
                var result = _validator.Validate(arg);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
