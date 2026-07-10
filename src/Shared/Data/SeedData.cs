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
                Category = MovieCategory.SpecialEvent,
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
                PopularityScore = 900,
            },
            new Movie
            {
                // Carried over from the running demo database (was added via /admin).
                Id = Guid.Parse("45e37bc7-a642-4c71-9410-62a7688834c5"),
                UrlTitle = "supergirl",
                Title = "Supergirl",
                Image = "supergirl.jpg",
                Background = "supergirl.jpg",
                Rating = 4,
                Description = "Bla bla bla",
                Icons = ["16"],
                MovieDetails = "127 minutes | English",
                ReleaseDate = new DateTime(2026, 7, 8),
                Showtimes = ["10:00"],
                PricePerTicket = 18,
                PopularityScore = 850,
            },
            new Movie
            {
                Id = DuneId,
                UrlTitle = "dune",
                Title = "Dune",
                Image = "dune.jpg",
                Background = "dune.jpg",
                Trailer = "https://www.youtube.com/watch?v=8g18jFHCLXk",
                Rating = 5,
                Description = "Feature adaptation of Frank Herbert's science fiction novel, about the son of a noble family entrusted with the protection of the most valuable asset and most vital element in the galaxy.",
                Icons = ["12", "violence", "fear"],
                MovieDetails = "156 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2021, 9, 16),
                Showtimes = ["10:00", "13:00", "15:00", "20:00", "23:00"],
                PricePerTicket = 20,
                PopularityScore = 800,
            },
            // The complete Star Wars saga. Posters/backgrounds are added later; the
            // image filenames follow the UrlTitle so they can be dropped in by name.
            .. StarWars(),
            // Special (non-film) events, surfaced in the "Specials & Events" section.
            new Movie
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000001"),
                UrlTitle = "titanic",
                Title = "Titanic",
                Image = "titanic.jpg",
                Background = "titanic.jpg",
                Rating = 5,
                Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic. Back on the big screen as a special event.",
                Icons = ["12"],
                MovieDetails = "194 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(1997, 12, 19),
                Showtimes = ["15:00", "19:30"],
                PricePerTicket = 15,
                PopularityScore = 500,
                Category = MovieCategory.SpecialEvent,
            },
            new Movie
            {
                Id = Guid.Parse("70000000-0000-0000-0000-000000000002"),
                UrlTitle = "guns-n-roses-use-your-illusion-tokyo",
                Title = "Guns N' Roses - Use Your Illusion in Tokyo",
                Image = "guns-n-roses-use-your-illusion-tokyo.jpg",
                Background = "guns-n-roses-use-your-illusion-tokyo.jpg",
                Rating = 5,
                Description = "The legendary Use Your Illusion I &amp; II concert, recorded live at the Tokyo Dome in 1992, on the big screen for one special music event.",
                Icons = ["12"],
                MovieDetails = "Concert film · 150 minutes",
                ReleaseDate = new DateTime(1992, 4, 22),
                Showtimes = ["20:30"],
                PricePerTicket = 22,
                PopularityScore = 490,
                Category = MovieCategory.MusicEvent,
            },
            new Movie
            {
                Id = Guid.Parse("60000000-0000-0000-0000-000000000001"),
                UrlTitle = "star-wars-marathon",
                Title = "Star Wars Marathon",
                Image = "star-wars-marathon.jpg",
                Background = "star-wars-marathon.jpg",
                Rating = 5,
                Description = "Experience the entire Skywalker saga back to back in one epic all-day marathon, from The Phantom Menace to The Rise of Skywalker.",
                Icons = ["9"],
                MovieDetails = "Marathon · 9 films | English, Dutch subtitles",
                ReleaseDate = new DateTime(2026, 8, 1),
                Showtimes = ["10:00"],
                PricePerTicket = 45,
                PopularityScore = 600,
                Category = MovieCategory.SpecialEvent,
            },
            // Upcoming premieres. ReleaseDate is in the future, so these are
            // flagged "Soon". Posters/backgrounds follow the UrlTitle, added later.
            new Movie
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000001"),
                UrlTitle = "toy-story-5",
                Title = "Toy Story 5",
                Image = "toy-story-5.jpg",
                Background = "toy-story-5.jpg",
                Rating = 5,
                Description = "Woody, Buzz and the gang are back for another adventure in the fifth chapter of Pixar's beloved saga.",
                Icons = [],
                MovieDetails = "100 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2026, 8, 15),
                Showtimes = ["13:00", "15:30", "18:00"],
                PricePerTicket = 12,
                PopularityScore = 780,
            },
            new Movie
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000002"),
                UrlTitle = "the-odyssey",
                Title = "The Odyssey",
                Image = "the-odyssey.jpg",
                Background = "the-odyssey.jpg",
                Rating = 5,
                Description = "Christopher Nolan's epic adaptation of Homer's myth, following Odysseus on his perilous journey home after the Trojan War.",
                Icons = ["12", "violence"],
                MovieDetails = "150 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2026, 7, 17),
                Showtimes = ["14:00", "20:00"],
                PricePerTicket = 16,
                PopularityScore = 770,
            },
            new Movie
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000003"),
                UrlTitle = "spider-man-brand-new-day",
                Title = "Spider-Man: Brand New Day",
                Image = "spider-man-brand-new-day.jpg",
                Background = "spider-man-brand-new-day.jpg",
                Rating = 4,
                Description = "Peter Parker swings back into action in a brand new chapter of the friendly neighbourhood Spider-Man.",
                Icons = ["12", "violence"],
                MovieDetails = "130 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2026, 7, 31),
                Showtimes = ["15:00", "18:30", "21:00"],
                PricePerTicket = 14,
                PopularityScore = 760,
            },
            new Movie
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000004"),
                UrlTitle = "star-wars-starfighter",
                Title = "Star Wars: Starfighter",
                Image = "star-wars-starfighter.jpg",
                Background = "star-wars-starfighter.jpg",
                Rating = 5,
                Description = "A new standalone adventure set in the Star Wars galaxy, following a band of pilots on the edge of the outer rim.",
                Icons = ["9"],
                MovieDetails = "135 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2027, 5, 28),
                Showtimes = ["13:30", "17:00", "20:30"],
                PricePerTicket = 13,
                PopularityScore = 750,
            },
            new Movie
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000005"),
                UrlTitle = "zero-ad",
                Title = "Zero A.D.",
                Image = "zero-ad.jpg",
                Background = "zero-ad.jpg",
                Rating = 4,
                Description = "A sweeping historical epic set at the dawn of a new era, where empires clash and a new world order is forged.",
                Icons = ["16", "violence"],
                MovieDetails = "128 minutes | English, Dutch subtitles",
                ReleaseDate = new DateTime(2026, 9, 25),
                Showtimes = ["16:00", "21:15"],
                PricePerTicket = 14,
                PopularityScore = 740,
            },
        ];

        // Episode order; release order lives in ReleaseDate. Rating uses the Dutch
        // kijkwijzer age icons ("9"/"12"). Episode IX reuses the existing poster.
        static IEnumerable<Movie> StarWars()
        {
            (string urlTitle, string title, int year, int month, int day, int minutes, string age, int score, string desc)[] episodes =
            [
                ("star-wars-the-phantom-menace", "Star Wars: Episode I - The Phantom Menace", 1999, 5, 19, 136, "9", 720, "Two Jedi escape a hostile blockade to find allies and come across a young boy who may bring balance to the Force."),
                ("star-wars-attack-of-the-clones", "Star Wars: Episode II - Attack of the Clones", 2002, 5, 16, 142, "9", 710, "Anakin Skywalker shares a forbidden romance with Padme while the Jedi uncover a secret clone army."),
                ("star-wars-revenge-of-the-sith", "Star Wars: Episode III - Revenge of the Sith", 2005, 5, 19, 140, "12", 700, "As the Clone Wars near an end, Anakin Skywalker is seduced by the dark side and becomes Darth Vader."),
                ("star-wars-a-new-hope", "Star Wars: Episode IV - A New Hope", 1977, 5, 25, 121, "9", 690, "Luke Skywalker joins forces with a Jedi Knight, a pilot and two droids to save the galaxy from the Empire's Death Star."),
                ("star-wars-the-empire-strikes-back", "Star Wars: Episode V - The Empire Strikes Back", 1980, 5, 21, 124, "9", 680, "After the Rebels are overpowered, Luke begins Jedi training with Yoda while his friends are pursued by Darth Vader."),
                ("star-wars-return-of-the-jedi", "Star Wars: Episode VI - Return of the Jedi", 1983, 5, 25, 131, "9", 670, "The Rebels dispatch to Endor to destroy a more powerful Death Star while Luke tries to turn Vader from the dark side."),
                ("star-wars-the-force-awakens", "Star Wars: Episode VII - The Force Awakens", 2015, 12, 16, 138, "9", 660, "Three decades after the Empire's defeat, a new threat arises as Rey, Finn and Han Solo search for the missing Luke Skywalker."),
                ("star-wars-the-last-jedi", "Star Wars: Episode VIII - The Last Jedi", 2017, 12, 13, 152, "12", 650, "Rey develops her abilities with Luke Skywalker as the Resistance prepares for battle with the First Order."),
                ("star-wars-the-rise-of-skywalker", "Star Wars: Episode IX - The Rise of Skywalker", 2019, 12, 18, 142, "12", 640, "The surviving Resistance faces the First Order once more in the final chapter of the Skywalker saga."),
            ];

            var index = 1;
            foreach (var (urlTitle, title, year, month, day, minutes, age, score, desc) in episodes)
            {
                var image = urlTitle == "star-wars-the-rise-of-skywalker" ? "riseofskywalker.jpg" : $"{urlTitle}.jpg";
                yield return new Movie
                {
                    Id = Guid.Parse($"e0000000-0000-0000-0000-00000000000{index}"),
                    UrlTitle = urlTitle,
                    Title = title,
                    Image = image,
                    Background = image,
                    Rating = 5,
                    Description = desc,
                    Icons = [age],
                    MovieDetails = $"{minutes} minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(year, month, day),
                    Showtimes = ["13:00", "16:30", "20:00"],
                    PricePerTicket = 12,
                    PopularityScore = score,
                };
                index++;
            }
        }

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
