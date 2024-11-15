using Microsoft.AspNetCore.Localization;

namespace BeHealthy.Endpoints.Culture;

public static class CultureEndpoints
{
    public static void MapCultureEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/Culture/Set");

        group.MapGet("", (string culture, string redirectUri, HttpContext httpContext) =>
        {
            if (culture != null)
            {
                var requestCulture = new RequestCulture(culture, culture);
                var cookieName = CookieRequestCultureProvider.DefaultCookieName;
                var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

                httpContext.Response.Cookies.Append(cookieName, cookieValue);
            }

            return TypedResults.LocalRedirect(redirectUri);
        });
    }
}
