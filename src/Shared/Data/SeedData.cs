using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Data
{
    // Code-based seeding, wired up through EF Core's UseSeeding/UseAsyncSeeding hooks
    // (see AddMoviesDatabase). Both are guarded so seeding runs only once.
    public static class SeedData
    {
        public static void Seed(DbContext context)
        {
            if (context.Set<Movie>().Any())
                return;

            context.Set<Movie>().AddRange(Movies());
            context.Set<Review>().AddRange(Reviews());
            context.SaveChanges();
        }

        public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            if (await context.Set<Movie>().AnyAsync(cancellationToken))
                return;

            await context.Set<Movie>().AddRangeAsync(Movies(), cancellationToken);
            await context.Set<Review>().AddRangeAsync(Reviews(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        static readonly Guid GameOfThronesId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        static readonly Guid DeadpoolId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        static readonly Guid DuneId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        static IEnumerable<Movie> Movies() =>
        [
            new Movie
            {
                Id = GameOfThronesId,
                UrlTitle = "gameofthrones",
                Title = "Game of Thrones",
                Image = "gameofthrones.jpg",
                Background = "gameofthrones.jpg",
                Rating = 5,
                Description = "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                Icons = ["16", "sex", "violence"],
                MovieDetails = "80 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2019, 5, 19),
                Showtimes = ["19:00"],
                PricePerTicket = 0,
                PopularityScore = 1000,
                TicketType = TicketType.DrawingTicket,
            },
            new Movie
            {
                Id = DeadpoolId,
                UrlTitle = "deadpool",
                Title = "Deadpool & Wolverine",
                Image = "deadpool.jpg",
                Background = "deadpool.jpg",
                Rating = 4,
                Description = "Deadpool is offered a place in the Marvel Cinematic Universe by the Time Variance Authority, but instead recruits a variant of Wolverine to save his universe from extinction.",
                Icons = ["16", "violence", "explicitlanguage"],
                MovieDetails = "127 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2024, 7, 24),
                Showtimes = ["10:00", "15:00", "20:00"],
                PricePerTicket = 10,
                PopularityScore = 200,
            },
            new Movie
            {
                Id = DuneId,
                UrlTitle = "dune",
                Title = "Dune",
                Image = "dune.jpg",
                Background = "dune.jpg",
                Rating = 5,
                Description = "Feature adaptation of Frank Herbert's science fiction novel, about the son of a noble family entrusted with the protection of the most valuable asset and most vital element in the galaxy.",
                Icons = ["12", "violence", "fear"],
                MovieDetails = "156 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2021, 9, 16),
                Showtimes = ["10:00", "13:00", "15:00", "20:00", "23:00"],
                PricePerTicket = 20,
                PopularityScore = 500,
            },
        ];

        static IEnumerable<Review> Reviews() =>
        [
            new Review { Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"), MovieIdentifier = GameOfThronesId, ReviewedAt = new DateTime(2019, 5, 19, 23, 30, 12), Description = "A riddle: A father and his son fight in the Battle of the Bastards. The father is killed in battle and the son is brought to the Warden of the North to be knighted. The Warden of the North looks at the boy and says: \"I can't knight him, he's my son!\" How can this be?" },
            new Review { Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"), MovieIdentifier = GameOfThronesId, ReviewedAt = new DateTime(2019, 5, 19, 23, 35, 36), Description = "I want more Arya! She's the best!" },
            new Review { Id = Guid.Parse("b1111111-1111-1111-1111-111111111111"), MovieIdentifier = DeadpoolId, ReviewedAt = new DateTime(2024, 7, 25, 12, 12, 12), Description = "Ryan Reynolds is hilarious again! Might just be the best Deadpool yet!" },
            new Review { Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"), MovieIdentifier = DeadpoolId, ReviewedAt = new DateTime(2024, 7, 26, 13, 45, 18), Description = "Loved it! Don't forget to wait for the past-credits scene! Never laughed so hard." },
            new Review { Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"), MovieIdentifier = DuneId, ReviewedAt = new DateTime(2021, 10, 8, 8, 14, 56), Description = "Great visuals, screenplay adaptation is too close to the books though." },
            new Review { Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"), MovieIdentifier = DuneId, ReviewedAt = new DateTime(2021, 10, 9, 13, 37, 0), Description = "Can't wait to go back to Mordor!" },
        ];
    }
}
