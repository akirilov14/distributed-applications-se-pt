using CineBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineBook.Infrastructure.Persistence.Configurations;

public class AuditoriumConfiguration : IEntityTypeConfiguration<Auditorium>
{
    public void Configure(EntityTypeBuilder<Auditorium> builder)
    {
        builder.HasKey(auditorium => auditorium.Id);

        builder.Property(auditorium => auditorium.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(auditorium => auditorium.Description)
            .HasMaxLength(500);

        builder.HasIndex(auditorium => auditorium.Name);
    }
}
