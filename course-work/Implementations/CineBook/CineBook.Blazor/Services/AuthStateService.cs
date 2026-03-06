using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace CineBook.Blazor.Services;

public class AuthStateService
{
    private string? _token;
    private string? _email;
    private string? _role;
    private string? _fullName;

    public event Action? OnAuthStateChanged;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public bool IsAdmin => _role == "Admin";
    public string? Token => _token;
    public string? Email => _email;
    public string? FullName => _fullName;

    public void SetUser(string token, string email, string role, string fullName)
    {
        _token = token;
        _email = email;
        _role = role;
        _fullName = fullName;
        OnAuthStateChanged?.Invoke();
    }

    public void ClearUser()
    {
        _token = null;
        _email = null;
        _role = null;
        _fullName = null;
        OnAuthStateChanged?.Invoke();
    }

    public ClaimsPrincipal GetClaimsPrincipal()
    {
        if (!IsAuthenticated)
            return new ClaimsPrincipal(new ClaimsIdentity());

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, _fullName ?? string.Empty),
            new(ClaimTypes.Email, _email ?? string.Empty),
            new(ClaimTypes.Role, _role ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, "JwtAuth");
        return new ClaimsPrincipal(identity);
    }
}

public class CineBookAuthStateProvider(AuthStateService authStateService) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = authStateService.GetClaimsPrincipal();
        return Task.FromResult(new AuthenticationState(principal));
    }

    public void NotifyStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
