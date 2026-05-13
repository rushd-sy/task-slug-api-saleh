using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SlugApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;

                if (validator is null) continue;

                var result = validator.Validate(new ValidationContext<object>(argument));
                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).ToArray()
                        );

                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
                    return;

                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
