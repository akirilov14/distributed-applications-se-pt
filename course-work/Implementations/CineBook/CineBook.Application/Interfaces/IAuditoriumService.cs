using CineBook.Application.Common;
using CineBook.Application.DTOs.Auditorium;

namespace CineBook.Application.Interfaces;

public interface IAuditoriumService
{
    Task<PagedResult<AuditoriumResponse>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<AuditoriumResponse>> SearchByNameAsync(string name, int pageNumber, int pageSize);
    Task<AuditoriumResponse?> GetByIdAsync(int id);
    Task<AuditoriumResponse> CreateAsync(CreateAuditoriumRequest request);
    Task<AuditoriumResponse?> UpdateAsync(int id, UpdateAuditoriumRequest request);
    Task<bool> DeleteAsync(int id);
}
