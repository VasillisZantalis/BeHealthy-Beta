namespace BeHealthy.Shared.Dtos.Common;

public record ServiceResponse(bool Success, string? ErrorMessage)
{
    public static ServiceResponse Successful() => new ServiceResponse(true, null);
    public static ServiceResponse Failed(string errorMessage = "Something went wrong") => new ServiceResponse(false, errorMessage);
}
