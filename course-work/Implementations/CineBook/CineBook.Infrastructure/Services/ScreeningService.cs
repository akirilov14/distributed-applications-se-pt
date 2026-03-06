using CineBook.Application.Common;
using CineBook.Application.DTOs.Screening;
using CineBook.Application.Interfaces;
using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Services;

public class ScreeningService(CineBookDbContext dbContext) : IScreeningService
{
    public async Task<PagedResult<ScreeningResponse>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Screenings
            .AsNoTracking()
            .Include(s => s.Film)
            .Include(s => s.Auditorium)
            .OrderBy(s => s.StartTime);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<ScreeningResponse>> SearchAsync(
        int? filmId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
    {
        var query = dbContext.Screenings
            .AsNoTracking()
            .Include(s => s.Film)
            .Include(s => s.Auditorium)
            .AsQueryable();

        if (filmId.HasValue)
            query = query.Where(s => s.FilmId == filmId.Value);

        if (startDate.HasValue)
        {
            var start = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            query = query.Where(s => s.StartTime >= start);
        }

        if (endDate.HasValue)
        {
            var end = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            query = query.Where(s => s.StartTime <= end);
        }

        query = query.OrderBy(s => s.StartTime);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<ScreeningResponse?> GetByIdAsync(int id)
    {
        var screening = await dbContext.Screenings
            .AsNoTracking()
            .Include(s => s.Film)
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == id);

        return screening is null ? null : MapToResponse(screening);
    }

    public async Task<ScreeningResponse> CreateAsync(CreateScreeningRequest request)
    {
        var film = await dbContext.Films.FindAsync(request.FilmId)
            ?? throw new InvalidOperationException("Филмът не е намерен.");

        var screening = new Screening
        {
            FilmId = request.FilmId,
            AuditoriumId = request.AuditoriumId,
            StartTime = request.StartTime,
            EndTime = request.StartTime.AddMinutes(film.DurationMinutes),
            BasePrice = request.BasePrice,
            Is3D = request.Is3D,
            Language = request.Language,
            Subtitles = request.Subtitles,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Screenings.Add(screening);
        await dbContext.SaveChangesAsync();

        await dbContext.Entry(screening).Reference(s => s.Film).LoadAsync();
        await dbContext.Entry(screening).Reference(s => s.Auditorium).LoadAsync();
        return MapToResponse(screening);
    }

    public async Task<ScreeningResponse?> UpdateAsync(int id, UpdateScreeningRequest request)
    {
        var screening = await dbContext.Screenings
            .Include(s => s.Film)
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (screening is null) return null;

        var film = await dbContext.Films.FindAsync(request.FilmId)
            ?? throw new InvalidOperationException("Филмът не е намерен.");

        screening.FilmId = request.FilmId;
        screening.AuditoriumId = request.AuditoriumId;
        screening.StartTime = request.StartTime;
        screening.EndTime = request.StartTime.AddMinutes(film.DurationMinutes);
        screening.BasePrice = request.BasePrice;
        screening.Is3D = request.Is3D;
        screening.Language = request.Language;
        screening.Subtitles = request.Subtitles;
        screening.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync();

        await dbContext.Entry(screening).Reference(s => s.Film).LoadAsync();
        await dbContext.Entry(screening).Reference(s => s.Auditorium).LoadAsync();
        return MapToResponse(screening);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var screening = await dbContext.Screenings.FindAsync(id);
        if (screening is null) return false;

        dbContext.Screenings.Remove(screening);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static async Task<PagedResult<ScreeningResponse>> ToPagedResultAsync(
        IQueryable<Screening> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => MapToResponse(s))
            .ToListAsync();

        return new PagedResult<ScreeningResponse>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        };
    }

    private static ScreeningResponse MapToResponse(Screening screening) => new()
    {
        Id = screening.Id,
        FilmId = screening.FilmId,
        FilmTitle = screening.Film?.Title ?? string.Empty,
        FilmDurationMinutes = screening.Film?.DurationMinutes ?? 0,
        AuditoriumId = screening.AuditoriumId,
        AuditoriumName = screening.Auditorium?.Name ?? string.Empty,
        StartTime = screening.StartTime,
        EndTime = screening.EndTime,
        BasePrice = screening.BasePrice,
        Is3D = screening.Is3D,
        Language = screening.Language,
        Subtitles = screening.Subtitles,
        IsActive = screening.IsActive,
        CreatedAt = screening.CreatedAt
    };
}
