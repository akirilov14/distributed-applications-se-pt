using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
{
    public void Configure(EntityTypeBuilder<Screening> builder)
    {
        builder.HasKey(screening => screening.Id);

        builder.Property(screening => screening.BasePrice)
            .HasPrecision(10, 2);

        builder.Property(screening => screening.Language)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(screening => screening.Subtitles)
            .HasMaxLength(50);

        builder.HasIndex(screening => screening.StartTime);

        builder.HasOne(screening => screening.Film)
            .WithMany(film => film.Screenings)
            .HasForeignKey(screening => screening.FilmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(screening => screening.Auditorium)
            .WithMany(auditorium => auditorium.Screenings)
            .HasForeignKey(screening => screening.AuditoriumId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
