using CineBook.Application.DTOs.Auth;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await authService.LoginAsync(request);
        if (response is null)
            return Unauthorized(new { message = "Невалиден имейл или парола." });

        return Ok(response);
    }
}
