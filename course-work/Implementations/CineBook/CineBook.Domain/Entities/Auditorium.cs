namespace CineBook.Domain.Entities;

public class Auditorium
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool Has3DProjector { get; set; }
    public bool HasDolbySound { get; set; }
    public int FloorNumber { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}