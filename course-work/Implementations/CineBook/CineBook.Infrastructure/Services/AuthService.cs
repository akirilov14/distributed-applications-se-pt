using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CineBook.Application.DTOs.Auth;
using CineBook.Application.Interfaces;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CineBook.Infrastructure.Services;

public class AuthService(CineBookDbContext dbContext, IConfiguration configuration) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await dbContext.AdminUsers
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (user is null)
            return null;

        bool passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordMatches)
            return null;

        user.LastLoginAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        var token = GenerateJwtToken(user.Email, user.Role, user.FullName);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    private string GenerateJwtToken(string email, string role, string fullName)
    {
        var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is not configured.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, fullName)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
