using CineBook.Application.Common;
using CineBook.Application.DTOs.Auditorium;
using CineBook.Application.Interfaces;
using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Services;

public class AuditoriumService(CineBookDbContext dbContext) : IAuditoriumService
{
    public async Task<PagedResult<AuditoriumResponse>> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = dbContext.Auditoriums.AsNoTracking().OrderBy(a => a.Name);
        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<PagedResult<AuditoriumResponse>> SearchByNameAsync(string name, int pageNumber, int pageSize)
    {
        var query = dbContext.Auditoriums
            .AsNoTracking()
            .Where(a => a.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(a => a.Name);

        return await ToPagedResultAsync(query, pageNumber, pageSize);
    }

    public async Task<AuditoriumResponse?> GetByIdAsync(int id)
    {
        var auditorium = await dbContext.Auditoriums.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        return auditorium is null ? null : MapToResponse(auditorium);
    }

    public async Task<AuditoriumResponse> CreateAsync(CreateAuditoriumRequest request)
    {
        var auditorium = new Auditorium
        {
            Name = request.Name,
            Capacity = request.Capacity,
            Has3DProjector = request.Has3DProjector,
            HasDolbySound = request.HasDolbySound,
            FloorNumber = request.FloorNumber,
            Description = request.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Auditoriums.Add(auditorium);
        await dbContext.SaveChangesAsync();
        return MapToResponse(auditorium);
    }

    public async Task<AuditoriumResponse?> UpdateAsync(int id, UpdateAuditoriumRequest request)
    {
        var auditorium = await dbContext.Auditoriums.FindAsync(id);
        if (auditorium is null) return null;

        auditorium.Name = request.Name;
        auditorium.Capacity = request.Capacity;
        auditorium.Has3DProjector = request.Has3DProjector;
        auditorium.HasDolbySound = request.HasDolbySound;
        auditorium.FloorNumber = request.FloorNumber;
        auditorium.Description = request.Description;
        auditorium.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync();
        return MapToResponse(auditorium);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var auditorium = await dbContext.Auditoriums.FindAsync(id);
        if (auditorium is null) return false;

        dbContext.Auditoriums.Remove(auditorium);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static async Task<PagedResult<AuditoriumResponse>> ToPagedResultAsync(
        IQueryable<Auditorium> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(a => MapToResponse(a))
            .ToListAsync();

        return new PagedResult<AuditoriumResponse>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Items = items
        };
    }

    private static AuditoriumResponse MapToResponse(Auditorium auditorium) => new()
    {
        Id = auditorium.Id,
        Name = auditorium.Name,
        Capacity = auditorium.Capacity,
        Has3DProjector = auditorium.Has3DProjector,
        HasDolbySound = auditorium.HasDolbySound,
        FloorNumber = auditorium.FloorNumber,
        Description = auditorium.Description,
        IsActive = auditorium.IsActive,
        CreatedAt = auditorium.CreatedAt
    };
}
