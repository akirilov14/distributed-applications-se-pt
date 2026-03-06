namespace CineBook.Domain.Entities;

public class ReservationSeat
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int SeatId { get; set; }
    public decimal Price { get; set; }
    public bool IsConfirmed { get; set; } = true;
    public string? SeatLabel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Reservation Reservation { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
}