using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Seat;

public class UpdateSeatRequest
{
    [Range(1, 100, ErrorMessage = "Номерът на реда трябва да е между 1 и 100.")]
    public int RowNumber { get; set; }

    [Range(1, 200, ErrorMessage = "Номерът на седалката трябва да е между 1 и 200.")]
    public int SeatNumber { get; set; }

    [Required(ErrorMessage = "Типът на седалката е задължителен.")]
    [MaxLength(50, ErrorMessage = "Типът не може да надвишава 50 символа.")]
    public string SeatType { get; set; } = "Standard";

    [Range(0.5, 5.0, ErrorMessage = "Множителят на цената трябва да е между 0.5 и 5.0.")]
    public decimal PriceMultiplier { get; set; } = 1.0m;

    public bool IsActive { get; set; } = true;
}