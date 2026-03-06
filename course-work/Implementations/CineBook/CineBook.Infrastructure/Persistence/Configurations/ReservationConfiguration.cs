using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(reservation => reservation.Id);

        builder.Property(reservation => reservation.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(reservation => reservation.CustomerEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(reservation => reservation.TotalPrice)
            .HasPrecision(10, 2);

        builder.Property(reservation => reservation.ReservationCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(reservation => reservation.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(reservation => reservation.Notes)
            .HasMaxLength(500);

        builder.HasIndex(reservation => reservation.ReservationCode)
            .IsUnique();

        builder.HasIndex(reservation => reservation.CustomerEmail);

        builder.HasOne(reservation => reservation.Screening)
            .WithMany(screening => screening.Reservations)
            .HasForeignKey(reservation => reservation.ScreeningId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
