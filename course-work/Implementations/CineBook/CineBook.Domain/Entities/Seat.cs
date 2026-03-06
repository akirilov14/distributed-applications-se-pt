namespace CineBook.Domain.Entities;

public class Seat
{
    public int Id { get; set; }
    public int AuditoriumId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = "Standard";
    public decimal PriceMultiplier { get; set; } = 1.0m;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Auditorium Auditorium { get; set; } = null!;
    public ICollection<ReservationSeat> ReservationSeats { get; set; } = new List<ReservationSeat>();
}