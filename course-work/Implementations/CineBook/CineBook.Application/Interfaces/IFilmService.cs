using CineBook.Application.Common;
using CineBook.Application.DTOs.Film;

namespace CineBook.Application.Interfaces;

public interface IFilmService
{
    Task<PagedResult<FilmResponse>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResult<FilmResponse>> SearchByTitleAsync(string title, int pageNumber, int pageSize);
    Task<FilmResponse?> GetByIdAsync(int id);
    Task<FilmResponse> CreateAsync(CreateFilmRequest request);
    Task<FilmResponse?> UpdateAsync(int id, UpdateFilmRequest request);
    Task<bool> DeleteAsync(int id);
}
