using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpassuDesafio.API.Filters
{
    public class FluentValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new List<ValidationFailure>();

            foreach (var argument in context.ActionArguments.Values.Where(argument => argument is not null))
            {
                var argumentType = argument!.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

                if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
                    continue;

                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                if (!validationResult.IsValid)
                    errors.AddRange(validationResult.Errors);
            }

            if (errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(errors.Select(error => error.ErrorMessage));
                return;
            }

            await next();
        }
    }
}
