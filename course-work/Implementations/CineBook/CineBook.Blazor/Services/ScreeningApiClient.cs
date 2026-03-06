using CineBook.Application.Common;
using CineBook.Application.DTOs.Screening;

namespace CineBook.Blazor.Services;

public class ScreeningApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<PagedResult<ScreeningResponse>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<ScreeningResponse>>($"api/screenings?pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<PagedResult<ScreeningResponse>?> SearchAsync(
        int? filmId, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
    {
        var queryParams = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (filmId.HasValue) queryParams.Add($"filmId={filmId.Value}");
        if (startDate.HasValue) queryParams.Add($"startDate={startDate.Value:yyyy-MM-ddTHH:mm:ss}");
        if (endDate.HasValue) queryParams.Add($"endDate={endDate.Value:yyyy-MM-ddTHH:mm:ss}");

        return GetAsync<PagedResult<ScreeningResponse>>($"api/screenings/search?{string.Join("&", queryParams)}");
    }

    public Task<ScreeningResponse?> GetByIdAsync(int id)
        => GetAsync<ScreeningResponse>($"api/screenings/{id}");

    public Task<ScreeningResponse?> CreateAsync(CreateScreeningRequest request)
        => PostAsync<CreateScreeningRequest, ScreeningResponse>("api/screenings", request);

    public Task<ScreeningResponse?> UpdateAsync(int id, UpdateScreeningRequest request)
        => PutAsync<UpdateScreeningRequest, ScreeningResponse>($"api/screenings/{id}", request);

    public Task<bool> DeleteAsync(int id)
        => DeleteAsync($"api/screenings/{id}");

    public Task<List<int>?> GetReservedSeatIdsAsync(int screeningId)
        => GetAsync<List<int>>($"api/screenings/{screeningId}/reserved-seats");
}
