using System.Net.Http.Headers;
using System.Net.Http.Json;
using CineBook.Application.Common;

namespace CineBook.Blazor.Services;

public abstract class ApiClientBase(HttpClient httpClient, AuthStateService authStateService)
{
    protected HttpClient HttpClient { get; } = httpClient;

    protected void SetAuthHeader()
    {
        if (authStateService.IsAuthenticated)
        {
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authStateService.Token);
        }
        else
        {
            HttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        SetAuthHeader();
        var response = await HttpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        SetAuthHeader();
        var response = await HttpClient.PostAsJsonAsync(url, body);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    protected async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest body)
    {
        SetAuthHeader();
        var response = await HttpClient.PutAsJsonAsync(url, body);
        if (!response.IsSuccessStatusCode) return default;
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    protected async Task<bool> DeleteAsync(string url)
    {
        SetAuthHeader();
        var response = await HttpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }

    protected async Task<(TResponse? Result, string? ErrorMessage)> PostWithErrorAsync<TRequest, TResponse>(
        string url, TRequest body)
    {
        SetAuthHeader();
        var response = await HttpClient.PostAsJsonAsync(url, body);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            return (result, null);
        }

        var error = await TryReadErrorMessageAsync(response);
        return (default, error);
    }

    private static async Task<string?> TryReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var errorBody = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            return errorBody?.Message ?? response.ReasonPhrase;
        }
        catch
        {
            return response.ReasonPhrase;
        }
    }
}

public class ApiErrorResponse
{
    public string? Message { get; set; }
}
