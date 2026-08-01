namespace BeHealthy.API.Controllers;

/// <summary>
/// Shared behavior for all BeHealthy.API controllers: consistent RFC 7807 problem responses
/// for failures reported by the application services.
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{
    protected ObjectResult ProblemFromServiceResponse(ServiceResponse response) => Problem(
        detail: response.ErrorMessage ?? "The request could not be completed.",
        statusCode: StatusCodes.Status400BadRequest,
        title: "Request failed");

    protected ObjectResult NotFoundProblem(string entityName, object id) => Problem(
        detail: $"{entityName} with id '{id}' was not found.",
        statusCode: StatusCodes.Status404NotFound,
        title: "Resource not found");

    /// <summary>
    /// Returns a 400 problem when the id in the route does not match the id in the request body; otherwise null.
    /// </summary>
    protected ObjectResult? EnsureMatchingId(int routeId, int bodyId)
    {
        if (routeId == bodyId)
            return null;

        return Problem(
            detail: $"Route id '{routeId}' does not match body id '{bodyId}'.",
            statusCode: StatusCodes.Status400BadRequest,
            title: "Id mismatch");
    }
}
