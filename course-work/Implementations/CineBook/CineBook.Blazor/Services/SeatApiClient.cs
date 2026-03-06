using CineBook.Application.Common;
using CineBook.Application.DTOs.Seat;

namespace CineBook.Blazor.Services;

public class SeatApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<PagedResult<SeatResponse>?> GetByAuditoriumAsync(int auditoriumId, int pageNumber = 1, int pageSize = 200)
        => GetAsync<PagedResult<SeatResponse>>($"api/seats/search?auditoriumId={auditoriumId}&pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<SeatResponse?> CreateAsync(CreateSeatRequest request)
        => PostAsync<CreateSeatRequest, SeatResponse>("api/seats", request);

    public Task<SeatResponse?> UpdateAsync(int id, UpdateSeatRequest request)
        => PutAsync<UpdateSeatRequest, SeatResponse>($"api/seats/{id}", request);

    public Task<bool> DeleteAsync(int id)
        => DeleteAsync($"api/seats/{id}");
}
