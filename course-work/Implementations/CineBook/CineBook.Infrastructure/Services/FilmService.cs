using CineBook.Application.Common;
using CineBook.Application.DTOs.Film;
using CineBook.Application.Interfaces;
using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Services;

public class FilmService(CineBookDbContext dbContext) : IFilmService
{
    public async Task<PagedResult<FilmResponse>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Films.AsNoTracking().OrderByDescending(f => f.CreatedAt);
        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<FilmResponse>> SearchByTitleAsync(string title, int pageNumber, int pageSize)
    {
        var query = dbContext.Films
            .AsNoTracking()
            .Where(f => f.Title.ToLower().Contains(title.ToLower()))
            .OrderByDescending(f => f.CreatedAt);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<FilmResponse?> GetByIdAsync(int id)
    {
        var film = await dbContext.Films.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        return film is null ? null : MapToResponse(film);
    }

    public async Task<FilmResponse> CreateAsync(CreateFilmRequest request)
    {
        var film = new Film
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            Genre = request.Genre,
            Director = request.Director,
            ReleaseYear = request.ReleaseYear,
            PosterUrl = request.PosterUrl,
            Rating = request.Rating,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Films.Add(film);
        await dbContext.SaveChangesAsync();

        return MapToResponse(film);
    }

    public async Task<FilmResponse?> UpdateAsync(int id, UpdateFilmRequest request)
    {
        var film = await dbContext.Films.FindAsync(id);
        if (film is null) return null;

        film.Title = request.Title;
        film.Description = request.Description;
        film.DurationMinutes = request.DurationMinutes;
        film.Genre = request.Genre;
        film.Director = request.Director;
        film.ReleaseYear = request.ReleaseYear;
        film.PosterUrl = request.PosterUrl;
        film.Rating = request.Rating;
        film.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync();
        return MapToResponse(film);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var film = await dbContext.Films.FindAsync(id);
        if (film is null) return false;

        dbContext.Films.Remove(film);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static async Task<PagedResult<FilmResponse>> ToPagedResultAsync(
        IQueryable<Film> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(f => MapToResponse(f))
            .ToListAsync();

        return new PagedResult<FilmResponse>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        };
    }

    private static FilmResponse MapToResponse(Film film) => new()
    {
        Id = film.Id,
        Title = film.Title,
        Description = film.Description,
        DurationMinutes = film.DurationMinutes,
        Genre = film.Genre,
        Director = film.Director,
        ReleaseYear = film.ReleaseYear,
        PosterUrl = film.PosterUrl,
        Rating = film.Rating,
        IsActive = film.IsActive,
        CreatedAt = film.CreatedAt
    };
}
