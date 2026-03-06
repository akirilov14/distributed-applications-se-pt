using CineBook.Application.Interfaces;
using CineBook.Infrastructure.Persistence;
using CineBook.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CineBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CineBookDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IAuditoriumService, AuditoriumService>();
        services.AddScoped<ISeatService, SeatService>();
        services.AddScoped<IScreeningService, ScreeningService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
