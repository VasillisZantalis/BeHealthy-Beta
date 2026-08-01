using FluentValidation.Results;

namespace BeHealthy.Frontend.Extensions;

public static class ValidationResultExtensions
{
    public static Dictionary<string, List<string>> GetErrorsGroupedByProperty(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToList()
            );
    }
}
