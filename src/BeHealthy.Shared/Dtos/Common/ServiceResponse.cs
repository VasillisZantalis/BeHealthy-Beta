namespace BeHealthy.Shared.Dtos.Common;

/// <param name="ValidationErrors">
/// Per-field messages, keyed by request property name, when the API rejected the request as
/// invalid (a 400 <c>ValidationProblemDetails</c>). <c>null</c> for any other outcome.
/// </param>
public record ServiceResponse(bool Success, string? ErrorMessage, IReadOnlyDictionary<string, string[]>? ValidationErrors = null)
{
    public static ServiceResponse Successful() => new ServiceResponse(true, null);
    public static ServiceResponse Failed(string errorMessage = "Something went wrong") => new ServiceResponse(false, errorMessage);

    /// <summary>A failure whose <see cref="ErrorMessage"/> lists every field message, for callers that only show a toast.</summary>
    public static ServiceResponse ValidationFailed(IReadOnlyDictionary<string, string[]> errors)
        => new ServiceResponse(false, string.Join(" ", errors.Values.SelectMany(messages => messages).Distinct()), errors);
}
