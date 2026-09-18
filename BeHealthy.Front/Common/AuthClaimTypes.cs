namespace BeHealthy.Front.Common;

/// <summary>Custom claim types stored in the Front's auth cookie alongside the standard ones.</summary>
public static class AuthClaimTypes
{
    /// <summary>Holds the JWT issued by BeHealthy.API so the Front can call the API on the user's behalf.</summary>
    public const string ApiToken = "behealthy_api_token";
}
