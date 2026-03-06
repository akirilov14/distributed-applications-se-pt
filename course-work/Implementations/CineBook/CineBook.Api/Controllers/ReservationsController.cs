using CineBook.Application.DTOs.Reservation;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await reservationService.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Search(
        [FromQuery] string email,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { message = "Полето за търсене не може да е празно." });

        var result = await reservationService.SearchByEmailAsync(email, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await reservationService.GetByIdAsync(id);
        if (reservation is null)
            return NotFound(new { message = "Резервацията не е намерена." });

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var reservation = await reservationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await reservationService.CancelAsync(id);
        if (!cancelled)
            return NotFound(new { message = "Резервацията не е намерена." });

        return NoContent();
    }
}
