namespace CineBook.Application.DTOs.Auditorium;

public class AuditoriumResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool Has3DProjector { get; set; }
    public bool HasDolbySound { get; set; }
    public int FloorNumber { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}