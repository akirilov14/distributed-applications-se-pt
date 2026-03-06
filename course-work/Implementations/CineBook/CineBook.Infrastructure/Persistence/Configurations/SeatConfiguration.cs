using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(seat => seat.Id);

        builder.Property(seat => seat.SeatType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(seat => seat.PriceMultiplier)
            .HasPrecision(4, 2);

        builder.HasIndex(seat => new { seat.AuditoriumId, seat.RowNumber, seat.SeatNumber })
            .IsUnique();

        builder.HasOne(seat => seat.Auditorium)
            .WithMany(auditorium => auditorium.Seats)
            .HasForeignKey(seat => seat.AuditoriumId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
