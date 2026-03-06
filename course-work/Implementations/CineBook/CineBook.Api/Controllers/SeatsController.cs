using CineBook.Application.DTOs.Seat;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SeatsController(ISeatService seatService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await seatService.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] int auditoriumId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 100)
    {
        if (auditoriumId <= 0)
            return BadRequest(new { message = "Невалиден идентификатор на зала." });

        var result = await seatService.GetByAuditoriumAsync(auditoriumId, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await seatService.GetByIdAsync(id);
        if (seat is null)
            return NotFound(new { message = "Седалката не е намерена." });

        return Ok(seat);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSeatRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var seat = await seatService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = seat.Id }, seat);
        }
        catch (Exception ex) when (ex.Message.Contains("unique"))
        {
            return Conflict(new { message = "Седалка с този ред и номер вече съществува в залата." });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSeatRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var seat = await seatService.UpdateAsync(id, request);
        if (seat is null)
            return NotFound(new { message = "Седалката не е намерена." });

        return Ok(seat);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await seatService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Седалката не е намерена." });

        return NoContent();
    }
}
