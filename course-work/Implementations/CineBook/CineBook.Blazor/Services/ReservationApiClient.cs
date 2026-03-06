using CineBook.Application.Common;
using CineBook.Application.DTOs.Reservation;

namespace CineBook.Blazor.Services;

public class ReservationApiClient(HttpClient httpClient, AuthStateService authStateService)
    : ApiClientBase(httpClient, authStateService)
{
    public Task<PagedResult<ReservationResponse>?> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<ReservationResponse>>($"api/reservations?pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<PagedResult<ReservationResponse>?> SearchByEmailAsync(string email, int pageNumber = 1, int pageSize = 10)
        => GetAsync<PagedResult<ReservationResponse>>($"api/reservations/search?email={Uri.EscapeDataString(email)}&pageNumber={pageNumber}&pageSize={pageSize}");

    public Task<ReservationResponse?> GetByIdAsync(int id)
        => GetAsync<ReservationResponse>($"api/reservations/{id}");

    public Task<(ReservationResponse? Result, string? ErrorMessage)> CreateAsync(CreateReservationRequest request)
        => PostWithErrorAsync<CreateReservationRequest, ReservationResponse>("api/reservations", request);

    public Task<bool> CancelAsync(int id)
        => DeleteAsync($"api/reservations/{id}");
}
