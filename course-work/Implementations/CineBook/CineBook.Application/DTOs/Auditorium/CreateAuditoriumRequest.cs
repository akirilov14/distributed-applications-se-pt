using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Auditorium;

public class CreateAuditoriumRequest
{
    [Required(ErrorMessage = "Името на залата е задължително.")]
    [MaxLength(100, ErrorMessage = "Името не може да надвишава 100 символа.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 2000, ErrorMessage = "Капацитетът трябва да е между 1 и 2000 места.")]
    public int Capacity { get; set; }

    public bool Has3DProjector { get; set; }

    public bool HasDolbySound { get; set; }

    [Range(0, 50, ErrorMessage = "Номерът на етажа трябва да е между 0 и 50.")]
    public int FloorNumber { get; set; }

    [MaxLength(500, ErrorMessage = "Описанието не може да надвишава 500 символа.")]
    public string? Description { get; set; }
}