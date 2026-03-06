using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class ReservationSeatConfiguration : IEntityTypeConfiguration<ReservationSeat>
{
    public void Configure(EntityTypeBuilder<ReservationSeat> builder)
    {
        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Price)
            .HasPrecision(10, 2);

        builder.Property(rs => rs.SeatLabel)
            .HasMaxLength(20);

        builder.HasIndex(rs => new { rs.ReservationId, rs.SeatId })
            .IsUnique();

        builder.HasOne(rs => rs.Reservation)
            .WithMany(reservation => reservation.ReservationSeats)
            .HasForeignKey(rs => rs.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rs => rs.Seat)
            .WithMany(seat => seat.ReservationSeats)
            .HasForeignKey(rs => rs.SeatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
