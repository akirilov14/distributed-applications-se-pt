namespace CineBook.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int ScreeningId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Screening Screening { get; set; } = null!;
    public ICollection<ReservationSeat> ReservationSeats { get; set; } = new List<ReservationSeat>();
}