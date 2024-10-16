using FluentValidation;

namespace BeHealthy.Filters;

public class ValidationFilter<TRequest> : IEndpointFilter
{
    private readonly IValidator<TRequest> _validator;

    public ValidationFilter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext contextFactory, EndpointFilterDelegate next)
    {
        var request = contextFactory.Arguments.OfType<TRequest>().First();

        var result = await _validator.ValidateAsync(request, contextFactory.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(contextFactory);
    }
}
