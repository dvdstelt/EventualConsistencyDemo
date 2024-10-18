using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Entities;

namespace Shared.Configuration
{
    static class DefaultData
    {
        public static IEnumerable<Movie> GetDefaultMovies()
        {
            return new List<Movie>()
            {
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "gameofthrones",
                    Title = "Game of Thrones",
                    Image = "gameofthrones.jpg",
                    Rating = 5,
                    Description =
                        "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                    Icons = new List<string> {"16", "sex", "violence"},
                    MovieDetails = "80 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 05, 19),
                    Showtimes = new List<string> {"19:00"},
                    PricePerTicket = 0D,
                    PopularityScore = 1000,
                    TicketType = TicketType.DrawingTicket,
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "deadpool",
                    Title = "Deadpool & Wolverine",
                    Image = "deadpool.jpg",
                    Rating = 4,
                    Description =
                        "Deadpool is offered a place in the Marvel Cinematic Universe by the Time Variance Authority, but instead recruits a variant of Wolverine to save his universe from extinction.",
                    Icons = new List<string> {"16", "violence", "explicitlanguage"},
                    MovieDetails = "127 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2024, 7, 24),
                    Showtimes = new List<string> {"10:00", "15:00", "20:00"},
                    PricePerTicket = 10D,
                    PopularityScore = 200,
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "dune",
                    Title = "Dune",
                    Image = "dune.jpg",
                    Rating = 5,
                    Description =
                        "Feature adaptation of Frank Herbert's science fiction novel, about the son of a noble family entrusted with the protection of the most valuable asset and most vital element in the galaxy.",
                    Icons = new List<string> {"12", "violence", "fear" },
                    MovieDetails = "156 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2021, 9, 16),
                    Showtimes = new List<string> {"10:00", "13:00", "15:00", "20:00", "23:00"},
                    PricePerTicket = 20D,
                    PopularityScore = 500,
                },
            };
        }

        public static List<Review> GetDefaultReviews(IEnumerable<Movie> movies)
        {
            var gotId = movies.Single(s => s.UrlTitle == "gameofthrones").Id;
            var deadpoolId = movies.Single(s => s.UrlTitle == "deadpool").Id;
            var dune = movies.Single(s => s.UrlTitle == "dune").Id;

            return new List<Review>
            {
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "A riddle: A father and his son fight in the Battle of the Bastards. The father is killed in battle and the son is brought to the Warden of the North to be knighted. The Warden of the North looks at the boy and says: \"I can't knight him, he's my son!\" How can this be?", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = deadpoolId, Description = "Ryan Reynolds is hilarious again! Might just be the best Deadpool yet!", ReviewedAt = new DateTime(2024, 7, 25, 12, 12, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = deadpoolId, Description = "Loved it! Don't forget to wait for the past-credits scene! Never laughed so hard.", ReviewedAt = new DateTime(2025, 7, 26, 13, 45, 18) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Great visuals, screenplay adaptation is too close to the books though.", ReviewedAt = new DateTime(2021, 10, 8, 8, 14, 56) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Can't wait to go back to Mordor!", ReviewedAt = new DateTime(2021, 10, 9, 13, 37, 00) },
            };
        }

    }
}
