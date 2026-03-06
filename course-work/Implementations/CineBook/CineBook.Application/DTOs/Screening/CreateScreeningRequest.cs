using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Screening;

public class CreateScreeningRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Филмът е задължителен.")]
    public int FilmId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Залата е задължителна.")]
    public int AuditoriumId { get; set; }

    [Required(ErrorMessage = "Началото на прожекцията е задължително.")]
    public DateTime StartTime { get; set; }

    [Range(0.01, 1000, ErrorMessage = "Базовата цена трябва да е между 0.01 и 1000 лв.")]
    public decimal BasePrice { get; set; }

    public bool Is3D { get; set; }

    [Required(ErrorMessage = "Езикът е задължителен.")]
    [MaxLength(50, ErrorMessage = "Езикът не може да надвишава 50 символа.")]
    public string Language { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Субтитрите не могат да надвишават 50 символа.")]
    public string? Subtitles { get; set; }
}