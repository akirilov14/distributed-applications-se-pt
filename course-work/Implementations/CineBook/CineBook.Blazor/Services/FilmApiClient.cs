using CineBook.Application.Common;
using CineBook.Application.DTOs.Film;

namespace CineBook.Blazor.Services;

public class FilmApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<PagedResult<FilmResponse>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<FilmResponse>>($"api/films?pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<PagedResult<FilmResponse>?> SearchAsync(string title, int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<FilmResponse>>($"api/films/search?title={Uri.EscapeDataString(title)}&pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<FilmResponse?> GetByIdAsync(int id)
        => GetAsync<FilmResponse>($"api/films/{id}");

    public Task<FilmResponse?> CreateAsync(CreateFilmRequest request)
        => PostAsync<CreateFilmRequest, FilmResponse>("api/films", request);

    public Task<FilmResponse?> UpdateAsync(int id, UpdateFilmRequest request)
        => PutAsync<UpdateFilmRequest, FilmResponse>($"api/films/{id}", request);

    public Task<bool> DeleteAsync(int id)
        => DeleteAsync($"api/films/{id}");
}
