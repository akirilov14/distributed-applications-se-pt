using CineBook.Application.Common;
using CineBook.Application.DTOs.Reservation;

namespace CineBook.Application.Interfaces;

public interface IReservationService
{
    Task<PagedResult<ReservationResponse>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<ReservationResponse>> SearchByEmailAsync(string email, int pageNumber, int pageSize);
    Task<ReservationResponse?> GetByIdAsync(int id);
    Task<ReservationResponse> CreateAsync(CreateReservationRequest request);
    Task<bool> CancelAsync(int id);
    Task<List<int>> GetReservedSeatIdsAsync(int screeningId);
}
