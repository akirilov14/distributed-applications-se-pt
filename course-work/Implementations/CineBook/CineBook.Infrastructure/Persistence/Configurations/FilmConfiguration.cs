using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.HasKey(film => film.Id);

        builder.Property(film => film.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(film => film.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(film => film.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(film => film.Director)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(film => film.PosterUrl)
            .HasMaxLength(500);

        builder.Property(film => film.Rating)
            .HasPrecision(4, 2);

        builder.HasIndex(film => film.Title);
    }
}
