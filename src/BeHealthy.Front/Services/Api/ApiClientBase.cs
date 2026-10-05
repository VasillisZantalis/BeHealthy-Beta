using BeHealthy.Front.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Front.Services.Api;

/// <summary>
/// Base class for the server-side API services. Wraps the named "API" <see cref="HttpClient"/>
/// and provides small helpers for the common request shapes used across the app.
/// </summary>
public abstract class ApiClientBase
{
    protected readonly HttpClient httpClient;

    protected ApiClientBase(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser)
    {
        httpClient = httpClientFactory.CreateClient("API");

        if (!string.IsNullOrEmpty(currentUser.Token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", currentUser.Token);
        }
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<T>(url, ApiJsonOptions.Default);
        }
        catch (HttpRequestException)
        {
            return default;
        }
    }

    protected async Task<List<T>> GetListAsync<T>(string url)
        => await GetAsync<List<T>>(url) ?? new List<T>();

    protected async Task<ServiceResponse> PostForResponseAsync<TBody>(string url, TBody body)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(url, body, ApiJsonOptions.Default);
            return await ReadServiceResponseAsync(response);
        }
        catch (HttpRequestException ex)
        {
            return ServiceResponse.Failed(ex.Message);
        }
    }

    protected async Task<ServiceResponse> PutForResponseAsync<TBody>(string url, TBody body)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync(url, body, ApiJsonOptions.Default);
            return await ReadServiceResponseAsync(response);
        }
        catch (HttpRequestException ex)
        {
            return ServiceResponse.Failed(ex.Message);
        }
    }

    protected async Task<ServiceResponse> DeleteForResponseAsync(string url)
    {
        try
        {
            var response = await httpClient.DeleteAsync(url);
            return await ReadServiceResponseAsync(response);
        }
        catch (HttpRequestException ex)
        {
            return ServiceResponse.Failed(ex.Message);
        }
    }

    protected async Task PostAsync<TBody>(string url, TBody body)
        => await httpClient.PostAsJsonAsync(url, body, ApiJsonOptions.Default);

    protected async Task PutAsync<TBody>(string url, TBody body)
        => await httpClient.PutAsJsonAsync(url, body, ApiJsonOptions.Default);

    protected async Task DeleteAsync(string url)
        => await httpClient.DeleteAsync(url);

    protected async Task<ServiceResponse> PatchForResponseAsync<TBody>(string url, TBody body)
    {
        try
        {
            var response = await httpClient.PatchAsJsonAsync(url, body, ApiJsonOptions.Default);
            return await ReadServiceResponseAsync(response);
        }
        catch (HttpRequestException ex)
        {
            return ServiceResponse.Failed(ex.Message);
        }
    }

    /// <summary>
    /// The API answers failures with RFC 7807 problem details. A 400 from validation carries
    /// per-field errors, which are returned in <see cref="ServiceResponse.ValidationErrors"/> so a
    /// form can show them next to the right inputs.
    /// </summary>
    private static async Task<ServiceResponse> ReadServiceResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return ServiceResponse.Successful();
        }

        try
        {
            var problem = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>(ApiJsonOptions.Default);

            if (problem?.Errors is { Count: > 0 } errors)
            {
                return ServiceResponse.ValidationFailed(errors.ToDictionary(e => e.Key, e => e.Value));
            }

            var message = problem?.Detail ?? problem?.Title;
            if (!string.IsNullOrWhiteSpace(message))
            {
                return ServiceResponse.Failed(message);
            }
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            // Not a problem details body (for example an empty 404): fall back to the status code.
        }

        return ServiceResponse.Failed($"Request failed with status {(int)response.StatusCode}");
    }

    protected static string ToQueryString(QueryParameters? parameters)
    {
        if (parameters is null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        void Add(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            sb.Append(sb.Length == 0 ? '?' : '&');
            sb.Append(Uri.EscapeDataString(key));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(value));
        }

        Add(nameof(parameters.SearchTerm), parameters.SearchTerm);
        Add(nameof(parameters.PageNumber), parameters.PageNumber.ToString());
        Add(nameof(parameters.PageSize), parameters.PageSize.ToString());
        Add(nameof(parameters.OrderBy), parameters.OrderBy);
        Add(nameof(parameters.OrderDescending), parameters.OrderDescending.ToString());

        switch (parameters)
        {
            case DoctorQueryParameters d:
                Add(nameof(d.SpecialtyId), d.SpecialtyId?.ToString());
                break;
            case PatientQueryParameters p:
                Add(nameof(p.FirstName), p.FirstName);
                Add(nameof(p.LastName), p.LastName);
                break;
            case AppointmentQueryParameters a:
                Add(nameof(a.DoctorId), a.DoctorId?.ToString());
                Add(nameof(a.PatientId), a.PatientId?.ToString());
                break;
        }

        return sb.ToString();
    }
}
