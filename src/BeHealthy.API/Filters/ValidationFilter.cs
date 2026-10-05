using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BeHealthy.API.Filters;

/// <summary>
/// Runs every registered FluentValidation validator for each action argument (request bodies and
/// [FromQuery] parameter objects) before the action executes. Registered globally, so no endpoint
/// can forget to validate. On failure the action never runs and the client gets a 400
/// <see cref="ValidationProblemDetails"/> with the errors grouped by property name, the same shape
/// [ApiController] uses for model-binding errors.
/// </summary>
/// <remarks>
/// A request type can have more than one validator: the shared one from BeHealthy.Validation and a
/// server-only one from BeHealthy.Application. All of them run and their errors are merged.
/// They run one after another because they share the request's DbContext.
/// </remarks>
public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var services = context.HttpContext.RequestServices;
        var cancellationToken = context.HttpContext.RequestAborted;
        var failures = new List<ValidationFailure>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            foreach (var validator in services.GetServices(validatorType).OfType<IValidator>())
            {
                var result = await validator.ValidateAsync(new ValidationContext<object>(argument), cancellationToken);
                failures.AddRange(result.Errors);
            }
        }

        if (failures.Count == 0)
        {
            await next();
            return;
        }

        foreach (var failure in failures)
        {
            context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }

        var problemDetailsFactory = services.GetRequiredService<ProblemDetailsFactory>();
        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState, StatusCodes.Status400BadRequest);

        context.Result = new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    }
}
