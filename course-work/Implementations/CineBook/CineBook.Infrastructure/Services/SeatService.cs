using CineBook.Application.Common;
using CineBook.Application.DTOs.Seat;
using CineBook.Application.Interfaces;
using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Services;

public class SeatService(CineBookDbContext dbContext) : ISeatService
{
    public async Task<PagedResult<SeatResponse>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Seats
            .AsNoTracking()
            .Include(s => s.Auditorium)
            .OrderBy(s => s.AuditoriumId)
            .ThenBy(s => s.RowNumber)
            .ThenBy(s => s.SeatNumber);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<SeatResponse>> GetByAuditoriumAsync(int auditoriumId, int pageNumber, int pageSize)
    {
        var query = dbContext.Seats
            .AsNoTracking()
            .Include(s => s.Auditorium)
            .Where(s => s.AuditoriumId == auditoriumId)
            .OrderBy(s => s.RowNumber)
            .ThenBy(s => s.SeatNumber);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<SeatResponse?> GetByIdAsync(int id)
    {
        var seat = await dbContext.Seats
            .AsNoTracking()
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == id);

        return seat is null ? null : MapToResponse(seat);
    }

    public async Task<SeatResponse> CreateAsync(CreateSeatRequest request)
    {
        var seat = new Seat
        {
            AuditoriumId = request.AuditoriumId,
            RowNumber = request.RowNumber,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType,
            PriceMultiplier = request.PriceMultiplier,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Seats.Add(seat);
        await dbContext.SaveChangesAsync();

        await dbContext.Entry(seat).Reference(s => s.Auditorium).LoadAsync();
        return MapToResponse(seat);
    }

    public async Task<SeatResponse?> UpdateAsync(int id, UpdateSeatRequest request)
    {
        var seat = await dbContext.Seats
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (seat is null) return null;

        seat.RowNumber = request.RowNumber;
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType;
        seat.PriceMultiplier = request.PriceMultiplier;
        seat.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync();
        return MapToResponse(seat);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await dbContext.Seats.FindAsync(id);
        if (seat is null) return false;

        dbContext.Seats.Remove(seat);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static async Task<PagedResult<SeatResponse>> ToPagedResultAsync(
        IQueryable<Seat> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => MapToResponse(s))
            .ToListAsync();

        return new PagedResult<SeatResponse>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        };
    }

    private static SeatResponse MapToResponse(Seat seat) => new()
    {
        Id = seat.Id,
        AuditoriumId = seat.AuditoriumId,
        AuditoriumName = seat.Auditorium?.Name ?? string.Empty,
        RowNumber = seat.RowNumber,
        SeatNumber = seat.SeatNumber,
        SeatType = seat.SeatType,
        PriceMultiplier = seat.PriceMultiplier,
        IsActive = seat.IsActive,
        CreatedAt = seat.CreatedAt
    };
}
