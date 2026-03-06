using CineBook.Application.DTOs.Screening;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ScreeningsController(IScreeningService screeningService, IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await screeningService.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] int? filmId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await screeningService.SearchAsync(filmId, startDate, endDate, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var screening = await screeningService.GetByIdAsync(id);
        if (screening is null)
            return NotFound(new { message = "Прожекцията не е намерена." });

        return Ok(screening);
    }

    [HttpGet("{id:int}/reserved-seats")]
    public async Task<IActionResult> GetReservedSeats(int id)
    {
        var seatIds = await reservationService.GetReservedSeatIdsAsync(id);
        return Ok(seatIds);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateScreeningRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var screening = await screeningService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = screening.Id }, screening);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateScreeningRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var screening = await screeningService.UpdateAsync(id, request);
            if (screening is null)
                return NotFound(new { message = "Прожекцията не е намерена." });

            return Ok(screening);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await screeningService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Прожекцията не е намерена." });

        return NoContent();
    }
}
