using CineBook.Application.Common;
using CineBook.Application.DTOs.Auditorium;

namespace CineBook.Blazor.Services;

public class AuditoriumApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<PagedResult<AuditoriumResponse>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<AuditoriumResponse>>($"api/auditoriums?pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<PagedResult<AuditoriumResponse>?> SearchAsync(string name, int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<AuditoriumResponse>>($"api/auditoriums/search?name={Uri.EscapeDataString(name)}&pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<AuditoriumResponse?> GetByIdAsync(int id)
        => GetAsync<AuditoriumResponse>($"api/auditoriums/{id}");

    public Task<AuditoriumResponse?> CreateAsync(CreateAuditoriumRequest request)
        => PostAsync<CreateAuditoriumRequest, AuditoriumResponse>("api/auditoriums", request);

    public Task<AuditoriumResponse?> UpdateAsync(int id, UpdateAuditoriumRequest request)
        => PutAsync<UpdateAuditoriumRequest, AuditoriumResponse>($"api/auditoriums/{id}", request);

    public Task<bool> DeleteAsync(int id)
        => DeleteAsync($"api/auditoriums/{id}");
}
