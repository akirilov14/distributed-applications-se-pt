using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Reservation;

public class CreateReservationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Прожекцията е задължителна.")]
    public int ScreeningId { get; set; }

    [Required(ErrorMessage = "Името на клиента е задължително.")]
    [MaxLength(200, ErrorMessage = "Името не може да надвишава 200 символа.")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имейлът на клиента е задължителен.")]
    [MaxLength(200, ErrorMessage = "Имейлът не може да надвишава 200 символа.")]
    [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
    public string CustomerEmail { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Бележката не може да надвишава 500 символа.")]
    public string? Notes { get; set; }

    [MinLength(1, ErrorMessage = "Трябва да изберете поне едно място.")]
    public List<int> SeatIds { get; set; } = [];
}
