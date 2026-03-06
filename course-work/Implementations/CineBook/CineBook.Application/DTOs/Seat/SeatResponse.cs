namespace CineBook.Application.DTOs.Seat;

public class SeatResponse
{
    public int Id { get; set; }
    public int AuditoriumId { get; set; }
    public string AuditoriumName { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = string.Empty;
    public decimal PriceMultiplier { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}