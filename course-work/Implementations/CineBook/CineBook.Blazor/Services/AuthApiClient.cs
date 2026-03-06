using CineBook.Application.DTOs.Auth;

namespace CineBook.Blazor.Services;

public class AuthApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<LoginResponse?> LoginAsync(LoginRequest request)
        => PostAsync<LoginRequest, LoginResponse>("api/auth/login", request);
}
