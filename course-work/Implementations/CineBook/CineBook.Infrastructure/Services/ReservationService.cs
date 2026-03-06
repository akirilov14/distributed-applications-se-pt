using CineBook.Application.Common;
using CineBook.Application.DTOs.Reservation;
using CineBook.Application.Interfaces;
using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Services;

public class ReservationService(CineBookDbContext dbContext) : IReservationService
{
    public async Task<PagedResult<ReservationResponse>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Screening).ThenInclude(s => s.Film)
            .Include(r => r.Screening).ThenInclude(s => s.Auditorium)
            .Include(r => r.ReservationSeats).ThenInclude(rs => rs.Seat)
            .OrderByDescending(r => r.CreatedAt);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<ReservationResponse>> SearchByEmailAsync(string email, int pageNumber, int pageSize)
    {
        var query = dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Screening).ThenInclude(s => s.Film)
            .Include(r => r.Screening).ThenInclude(s => s.Auditorium)
            .Include(r => r.ReservationSeats).ThenInclude(rs => rs.Seat)
            .Where(r => r.CustomerEmail.ToLower().Contains(email.ToLower()))
            .OrderByDescending(r => r.CreatedAt);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<ReservationResponse?> GetByIdAsync(int id)
    {
        var reservation = await dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Screening).ThenInclude(s => s.Film)
            .Include(r => r.Screening).ThenInclude(s => s.Auditorium)
            .Include(r => r.ReservationSeats).ThenInclude(rs => rs.Seat)
            .FirstOrDefaultAsync(r => r.Id == id);

        return reservation is null ? null : MapToResponse(reservation);
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request)
    {
        var screening = await dbContext.Screenings
            .Include(s => s.Film)
            .Include(s => s.Auditorium)
            .FirstOrDefaultAsync(s => s.Id == request.ScreeningId)
            ?? throw new InvalidOperationException("Прожекцията не е намерена.");

        if (!screening.IsActive)
            throw new InvalidOperationException("Прожекцията не е активна.");

        var seats = await dbContext.Seats
            .Where(s => request.SeatIds.Contains(s.Id) && s.AuditoriumId == screening.AuditoriumId && s.IsActive)
            .ToListAsync();

        if (seats.Count != request.SeatIds.Count)
            throw new InvalidOperationException("Едно или повече места не съществуват или не са активни.");

        var alreadyReservedSeatIds = await dbContext.ReservationSeats
            .Include(rs => rs.Reservation)
            .Where(rs => request.SeatIds.Contains(rs.SeatId)
                && rs.Reservation.ScreeningId == request.ScreeningId
                && rs.Reservation.Status == "Active")
            .Select(rs => rs.SeatId)
            .ToListAsync();

        if (alreadyReservedSeatIds.Count > 0)
            throw new InvalidOperationException("Едно или повече избрани места вече са резервирани.");

        var reservationCode = GenerateReservationCode();
        var totalPrice = seats.Sum(seat => screening.BasePrice * seat.PriceMultiplier);

        var reservation = new Reservation
        {
            ScreeningId = request.ScreeningId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            TotalPrice = totalPrice,
            ReservationCode = reservationCode,
            Status = "Active",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Reservations.Add(reservation);
        await dbContext.SaveChangesAsync();

        var reservationSeats = seats.Select(seat => new ReservationSeat
        {
            ReservationId = reservation.Id,
            SeatId = seat.Id,
            Price = screening.BasePrice * seat.PriceMultiplier,
            IsConfirmed = true,
            SeatLabel = $"Ред {seat.RowNumber}, Място {seat.SeatNumber}",
            CreatedAt = DateTime.UtcNow
        }).ToList();

        dbContext.ReservationSeats.AddRange(reservationSeats);
        await dbContext.SaveChangesAsync();

        return (await GetByIdAsync(reservation.Id))!;
    }

    public async Task<bool> CancelAsync(int id)
    {
        var reservation = await dbContext.Reservations.FindAsync(id);
        if (reservation is null) return false;

        reservation.Status = "Cancelled";
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<int>> GetReservedSeatIdsAsync(int screeningId)
    {
        return await dbContext.ReservationSeats
            .Include(rs => rs.Reservation)
            .Where(rs => rs.Reservation.ScreeningId == screeningId && rs.Reservation.Status == "Active")
            .Select(rs => rs.SeatId)
            .ToListAsync();
    }

    private static async Task<PagedResult<ReservationResponse>> ToPagedResultAsync(
        IQueryable<Reservation> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<ReservationResponse>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items.Select(MapToResponse).ToList()
        };
    }

    private static ReservationResponse MapToResponse(Reservation reservation) => new()
    {
        Id = reservation.Id,
        ScreeningId = reservation.ScreeningId,
        FilmTitle = reservation.Screening?.Film?.Title ?? string.Empty,
        ScreeningStartTime = reservation.Screening?.StartTime ?? default,
        AuditoriumName = reservation.Screening?.Auditorium?.Name ?? string.Empty,
        CustomerName = reservation.CustomerName,
        CustomerEmail = reservation.CustomerEmail,
        TotalPrice = reservation.TotalPrice,
        ReservationCode = reservation.ReservationCode,
        Status = reservation.Status,
        Notes = reservation.Notes,
        CreatedAt = reservation.CreatedAt,
        Seats = reservation.ReservationSeats.Select(rs => new ReservationSeatInfo
        {
            SeatId = rs.SeatId,
            RowNumber = rs.Seat?.RowNumber ?? 0,
            SeatNumber = rs.Seat?.SeatNumber ?? 0,
            SeatType = rs.Seat?.SeatType ?? string.Empty,
            Price = rs.Price
        }).ToList()
    };

    private static string GenerateReservationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new char[10];
        for (int i = 0; i < result.Length; i++)
            result[i] = chars[random.Next(chars.Length)];
        return new string(result);
    }
}
