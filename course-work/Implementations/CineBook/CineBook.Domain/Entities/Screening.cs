namespace CineBook.Domain.Entities;

public class Screening
{
    public int Id { get; set; }
    public int FilmId { get; set; }
    public int AuditoriumId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal BasePrice { get; set; }
    public bool Is3D { get; set; }
    public string Language { get; set; } = string.Empty;
    public string? Subtitles { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Film Film { get; set; } = null!;
    public Auditorium Auditorium { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}