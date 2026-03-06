using CineBook.Application.DTOs.Auditorium;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuditoriumsController(IAuditoriumService auditoriumService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await auditoriumService.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string name,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest(new { message = "Полето за търсене не може да е празно." });

        var result = await auditoriumService.SearchByNameAsync(name, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var auditorium = await auditoriumService.GetByIdAsync(id);
        if (auditorium is null)
            return NotFound(new { message = "Залата не е намерена." });

        return Ok(auditorium);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateAuditoriumRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var auditorium = await auditoriumService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = auditorium.Id }, auditorium);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAuditoriumRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var auditorium = await auditoriumService.UpdateAsync(id, request);
        if (auditorium is null)
            return NotFound(new { message = "Залата не е намерена." });

        return Ok(auditorium);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await auditoriumService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Залата не е намерена." });

        return NoContent();
    }
}
