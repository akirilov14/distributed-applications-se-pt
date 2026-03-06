namespace CineBook.Application.DTOs.Screening;

public class ScreeningResponse
{
    public int Id { get; set; }
    public int FilmId { get; set; }
    public string FilmTitle { get; set; } = string.Empty;
    public int FilmDurationMinutes { get; set; }
    public int AuditoriumId { get; set; }
    public string AuditoriumName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal BasePrice { get; set; }
    public bool Is3D { get; set; }
    public string Language { get; set; } = string.Empty;
    public string? Subtitles { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}