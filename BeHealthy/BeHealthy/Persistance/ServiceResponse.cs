namespace BeHealthy.Persistance;

public record ServiceResponse(bool Success, string? ErrorMessage)
{
    public static ServiceResponse Successful() => new ServiceResponse(true, null);
    public static ServiceResponse Failed(string errorMessage) => new ServiceResponse(false, errorMessage);
}
