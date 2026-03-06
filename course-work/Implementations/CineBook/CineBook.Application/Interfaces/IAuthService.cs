using CineBook.Application.DTOs.Auth;

namespace CineBook.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
