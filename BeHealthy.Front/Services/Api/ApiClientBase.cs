using BeHealthy.Front.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", currentUser.Token);
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

    private static async Task<ServiceResponse> ReadServiceResponseAsync(HttpResponseMessage response)
    {
        try
        {
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse>(ApiJsonOptions.Default);
            if (result is not null)
                return result;
        }
        catch
        {
            // fall through to status-based result
        }

        return response.IsSuccessStatusCode
            ? ServiceResponse.Successful()
            : ServiceResponse.Failed($"Request failed with status {(int)response.StatusCode}");
    }

    protected static string ToQueryString(QueryParameters? parameters)
    {
        if (parameters is null)
            return string.Empty;

        var sb = new StringBuilder();

        void Add(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
                return;
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
