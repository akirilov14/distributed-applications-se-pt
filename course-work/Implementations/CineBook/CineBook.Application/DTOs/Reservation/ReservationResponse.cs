namespace CineBook.Application.DTOs.Reservation;

public class ReservationResponse
{
    public int Id { get; set; }
    public int ScreeningId { get; set; }
    public string FilmTitle { get; set; } = string.Empty;
    public DateTime ScreeningStartTime { get; set; }
    public string AuditoriumName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ReservationSeatInfo> Seats { get; set; } = [];
}

public class ReservationSeatInfo
{
    public int SeatId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
