using CineBook.Application.Common;
using CineBook.Application.DTOs.Screening;

namespace CineBook.Application.Interfaces;

public interface IScreeningService
{
    Task<PagedResult<ScreeningResponse>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<ScreeningResponse>> SearchAsync(int? filmId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
    Task<ScreeningResponse?> GetByIdAsync(int id);
    Task<ScreeningResponse> CreateAsync(CreateScreeningRequest request);
    Task<ScreeningResponse?> UpdateAsync(int id, UpdateScreeningRequest request);
    Task<bool> DeleteAsync(int id);
}
