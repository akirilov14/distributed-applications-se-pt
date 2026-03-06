using CineBook.Application.Common;
using CineBook.Application.DTOs.Seat;

namespace CineBook.Application.Interfaces;

public interface ISeatService
{
    Task<PagedResult<SeatResponse>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<SeatResponse>> GetByAuditoriumAsync(int auditoriumId, int pageNumber, int pageSize);
    Task<SeatResponse?> GetByIdAsync(int id);
    Task<SeatResponse> CreateAsync(CreateSeatRequest request);
    Task<SeatResponse?> UpdateAsync(int id, UpdateSeatRequest request);
    Task<bool> DeleteAsync(int id);
}
