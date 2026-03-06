using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Film;

public class UpdateFilmRequest
{
    [Required(ErrorMessage = "Заглавието е задължително.")]
    [MaxLength(200, ErrorMessage = "Заглавието не може да надвишава 200 символа.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описанието е задължително.")]
    [MaxLength(2000, ErrorMessage = "Описанието не може да надвишава 2000 символа.")]
    public string Description { get; set; } = string.Empty;

    [Range(1, 600, ErrorMessage = "Продължителността трябва да е между 1 и 600 минути.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Жанрът е задължителен.")]
    [MaxLength(100, ErrorMessage = "Жанрът не може да надвишава 100 символа.")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Режисьорът е задължителен.")]
    [MaxLength(150, ErrorMessage = "Режисьорът не може да надвишава 150 символа.")]
    public string Director { get; set; } = string.Empty;

    [Range(1888, 2100, ErrorMessage = "Годината на излизане трябва да е валидна.")]
    public int ReleaseYear { get; set; }

    [MaxLength(500, ErrorMessage = "URL адресът на постера не може да надвишава 500 символа.")]
    public string? PosterUrl { get; set; }

    [Range(0, 10, ErrorMessage = "Оценката трябва да е между 0 и 10.")]
    public decimal Rating { get; set; }

    public bool IsActive { get; set; } = true;
}