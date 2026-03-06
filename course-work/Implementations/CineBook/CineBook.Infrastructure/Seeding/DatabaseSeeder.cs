using CineBook.Domain.Entities;
using CineBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Infrastructure.Seeding;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(CineBookDbContext dbContext)
    {
        await SeedAdminUserAsync(dbContext);
        await SeedFilmsAsync(dbContext);
        await SeedAuditoriumsAsync(dbContext);
        await SeedSeatsAsync(dbContext);
        await SeedScreeningsAsync(dbContext);
    }

    private static async Task SeedAdminUserAsync(CineBookDbContext dbContext)
    {
        if (await dbContext.AdminUsers.AnyAsync(u => u.Email == "admin@cinebook.bg"))
            return;

        dbContext.AdminUsers.Add(new AdminUser
        {
            Email = "admin@cinebook.bg",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "Admin",
            FullName = "Системен Администратор",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedFilmsAsync(CineBookDbContext dbContext)
    {
        if (await dbContext.Films.AnyAsync())
        {
            await PatchPosterUrlsAsync(dbContext);
            return;
        }

        var films = new List<Film>
        {
            new()
            {
                Title = "The Shawshank Redemption",
                Description = "Банкерът Анди Дюфрейн е осъден на доживотен затвор за убийството на съпругата си и нейния любовник. В затвора Шоушанк той се сприятелява с Ред и бавно печели уважението на затворниците и надзирателите, докато търпеливо планира своето бягство.",
                DurationMinutes = 142,
                Genre = "Драма",
                Director = "Франк Дарабонт",
                ReleaseYear = 1994,
                Rating = 9.3m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/8ERqVgbD5JM1DxqeMH89LQf5Wb5.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "The Dark Knight",
                Description = "Батман влиза в битка срещу Жокера – хаотичен престъпник, сеещ анархия из Готъм Сити. Противопоставянето им превръща се в психологически дуел, изправящ героя пред невъзможен морален избор.",
                DurationMinutes = 152,
                Genre = "Екшън",
                Director = "Кристофър Нолан",
                ReleaseYear = 2008,
                Rating = 9.0m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/m3N6ejIhoD160vgsOfRpBMguADg.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "Inception",
                Description = "Дом Коб е крадец с рядкото умение да влиза в сънищата на хората и да открадва тайните им. Той получава шанс за изкупление, ако успее да извърши обратното – да засади идея в нечий ум, операция позната като inception.",
                DurationMinutes = 148,
                Genre = "Научна фантастика",
                Director = "Кристофър Нолан",
                ReleaseYear = 2010,
                Rating = 8.8m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/rxHM1Cyn0cpgF7l9DVA2m3SS5Zk.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "Pulp Fiction",
                Description = "Преплетени истории на двама убийци наемници, боксьор, съпруга на гангстер и двама дребни бандити, разказани в нелинеен маниер с черен хумор и сурово насилие.",
                DurationMinutes = 154,
                Genre = "Криминален",
                Director = "Куентин Тарантино",
                ReleaseYear = 1994,
                Rating = 8.9m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/akzDuLoVw1RwG3nv92QOoB2VgBz.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "Interstellar",
                Description = "Земята е на ръба на гибелта. Бивш пилот на НАСА повежда група астронавти през червей близо до Сатурн в търсене на нова обитаема планета, докато се бори с времето, гравитацията и разстоянието от семейството си.",
                DurationMinutes = 169,
                Genre = "Научна фантастика",
                Director = "Кристофър Нолан",
                ReleaseYear = 2014,
                Rating = 8.7m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/omKeIQBr8oiW3MikuoGPQ6z55EG.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "Oppenheimer",
                Description = "Биографичен трилър за Дж. Робърт Опенхаймър – физикът, ръководил Манхатънския проект за създаването на атомната бомба по време на Втората световна война, и моралните последствия от неговото дело.",
                DurationMinutes = 180,
                Genre = "Биографичен",
                Director = "Кристофър Нолан",
                ReleaseYear = 2023,
                Rating = 8.2m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/ooOGz4YyBgp7EewnnpavQISCr25.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Title = "Dune: Part Two",
                Description = "Пол Атреидес се обединява с племето фремен и тръгва на война срещу заговорниците, унищожили семейството му. Изправен пред избор между любовта и съдбата на вселената, той трябва да предотврати страшно бъдеще.",
                DurationMinutes = 166,
                Genre = "Приключенски",
                Director = "Дени Вилньов",
                ReleaseYear = 2024,
                Rating = 8.4m,
                PosterUrl = "https://image.tmdb.org/t/p/w500/7aNjJYZanHr7Cvu4DBPwyWYaPzY.jpg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        dbContext.Films.AddRange(films);
        await dbContext.SaveChangesAsync();
    }

    private static async Task PatchPosterUrlsAsync(CineBookDbContext dbContext)
    {
        var posters = new Dictionary<string, string>
        {
            ["The Shawshank Redemption"] = "https://image.tmdb.org/t/p/w500/8ERqVgbD5JM1DxqeMH89LQf5Wb5.jpg",
            ["The Dark Knight"]          = "https://image.tmdb.org/t/p/w500/m3N6ejIhoD160vgsOfRpBMguADg.jpg",
            ["Inception"]                = "https://image.tmdb.org/t/p/w500/rxHM1Cyn0cpgF7l9DVA2m3SS5Zk.jpg",
            ["Pulp Fiction"]             = "https://image.tmdb.org/t/p/w500/akzDuLoVw1RwG3nv92QOoB2VgBz.jpg",
            ["Interstellar"]             = "https://image.tmdb.org/t/p/w500/omKeIQBr8oiW3MikuoGPQ6z55EG.jpg",
            ["Oppenheimer"]              = "https://image.tmdb.org/t/p/w500/ooOGz4YyBgp7EewnnpavQISCr25.jpg",
            ["Dune: Part Two"]           = "https://image.tmdb.org/t/p/w500/7aNjJYZanHr7Cvu4DBPwyWYaPzY.jpg",
        };

        var films = await dbContext.Films
            .Where(f => f.PosterUrl == null)
            .ToListAsync();

        if (films.Count == 0) return;

        foreach (var film in films)
        {
            if (posters.TryGetValue(film.Title, out var url))
                film.PosterUrl = url;
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedAuditoriumsAsync(CineBookDbContext dbContext)
    {
        if (await dbContext.Auditoriums.AnyAsync())
            return;

        dbContext.Auditoriums.AddRange(
            new Auditorium
            {
                Name = "Зала Алфа",
                Capacity = 120,
                Has3DProjector = false,
                HasDolbySound = true,
                FloorNumber = 1,
                Description = "Основна зала с Dolby Surround звук.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Auditorium
            {
                Name = "Зала Бета",
                Capacity = 80,
                Has3DProjector = true,
                HasDolbySound = true,
                FloorNumber = 2,
                Description = "Зала с 3D прожектор и Dolby Atmos звук.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Auditorium
            {
                Name = "Зала Гама",
                Capacity = 60,
                Has3DProjector = false,
                HasDolbySound = false,
                FloorNumber = 1,
                Description = "Малка уютна зала.",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedSeatsAsync(CineBookDbContext dbContext)
    {
        if (await dbContext.Seats.AnyAsync())
            return;

        var auditoriums = await dbContext.Auditoriums.OrderBy(a => a.Id).ToListAsync();
        if (auditoriums.Count == 0) return;

        // (rows, seatsPerRow, vipFromRow)
        var layouts = new (int Rows, int SeatsPerRow, int VipFromRow)[]
        {
            (10, 12, 9),  // Зала Алфа  – 120 места, VIP редове 9-10
            ( 8, 10, 7),  // Зала Бета  –  80 места, VIP редове 7-8
            ( 6, 10, 6),  // Зала Гама  –  60 места, VIP ред 6
        };

        var seats = new List<Seat>();

        for (int i = 0; i < auditoriums.Count && i < layouts.Length; i++)
        {
            var (rows, seatsPerRow, vipFromRow) = layouts[i];
            var auditoriumId = auditoriums[i].Id;

            for (int row = 1; row <= rows; row++)
            {
                for (int seat = 1; seat <= seatsPerRow; seat++)
                {
                    // First seat of first row is wheelchair-accessible
                    var isDisabled = row == 1 && seat == 1;
                    var isVip = !isDisabled && row >= vipFromRow;

                    seats.Add(new Seat
                    {
                        AuditoriumId = auditoriumId,
                        RowNumber = row,
                        SeatNumber = seat,
                        SeatType = isDisabled ? "Disabled" : isVip ? "VIP" : "Standard",
                        PriceMultiplier = isDisabled ? 0.8m : isVip ? 1.5m : 1.0m,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        dbContext.Seats.AddRange(seats);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedScreeningsAsync(CineBookDbContext dbContext)
    {
        if (await dbContext.Screenings.AnyAsync(s => s.StartTime >= DateTime.UtcNow))
            return;

        var expired = await dbContext.Screenings
            .Where(s => s.StartTime < DateTime.UtcNow)
            .ToListAsync();
        dbContext.Screenings.RemoveRange(expired);

        var films = await dbContext.Films.OrderBy(f => f.Id).ToListAsync();
        var auditoriums = await dbContext.Auditoriums.OrderBy(a => a.Id).ToListAsync();

        if (films.Count == 0 || auditoriums.Count == 0)
            return;

        var alfa = auditoriums[0];
        var beta = auditoriums[1];
        var gama = auditoriums.Count > 2 ? auditoriums[2] : auditoriums[0];

        var now = DateTime.UtcNow;
        var tomorrow = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);

        var schedule = new (string FilmTitle, int Hour, int Minute, Auditorium Hall, bool Is3D, decimal Price)[]
        {
            ("The Shawshank Redemption", 12,  0,  alfa, false, 12m),
            ("The Dark Knight",          15,  0,  alfa, false, 14m),
            ("Inception",                18,  0,  alfa, false, 14m),
            ("Pulp Fiction",             14, 30,  gama, false, 12m),
            ("Interstellar",             17,  0,  gama, false, 13m),
            ("Oppenheimer",              13,  0,  beta, false, 15m),
            ("Dune: Part Two",           17, 30,  beta, true,  18m),
        };

        var screenings = new List<Screening>();
        foreach (var entry in schedule)
        {
            var film = films.FirstOrDefault(f => f.Title == entry.FilmTitle);
            if (film is null) continue;

            var startTime = tomorrow.AddHours(entry.Hour).AddMinutes(entry.Minute);

            screenings.Add(new Screening
            {
                FilmId = film.Id,
                AuditoriumId = entry.Hall.Id,
                StartTime = startTime,
                EndTime = startTime.AddMinutes(film.DurationMinutes),
                BasePrice = entry.Price,
                Is3D = entry.Is3D,
                Language = "Английски",
                Subtitles = "Български",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        dbContext.Screenings.AddRange(screenings);
        await dbContext.SaveChangesAsync();
    }
}