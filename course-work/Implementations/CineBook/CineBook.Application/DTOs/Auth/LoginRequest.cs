using System.ComponentModel.DataAnnotations;

namespace CineBook.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Имейлът е задължителен.")]
    [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Паролата е задължителна.")]
    public string Password { get; set; } = string.Empty;
}
