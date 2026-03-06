using CineBook.Application.DTOs.Film;
using CineBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FilmsController(IFilmService filmService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await filmService.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string title,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest(new { message = "Полето за търсене не може да е празно." });

        var result = await filmService.SearchByTitleAsync(title, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var film = await filmService.GetByIdAsync(id);
        if (film is null)
            return NotFound(new { message = "Филмът не е намерен." });

        return Ok(film);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateFilmRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var film = await filmService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = film.Id }, film);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFilmRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var film = await filmService.UpdateAsync(id, request);
        if (film is null)
            return NotFound(new { message = "Филмът не е намерен." });

        return Ok(film);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await filmService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Филмът не е намерен." });

        return NoContent();
    }
}
