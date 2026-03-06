using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Persistence;

public class CineBookDbContext(DbContextOptions<CineBookDbContext> options) : DbContext(options)
{
    public DbSet<Film> Films => Set<Film>();
    public DbSet<Auditorium> Auditoriums => Set<Auditorium>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Screening> Screenings => Set<Screening>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationSeat> ReservationSeats => Set<ReservationSeat>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CineBookDbContext).Assembly);
    }
}
