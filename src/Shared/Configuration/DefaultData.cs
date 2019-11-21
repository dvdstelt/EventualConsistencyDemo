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
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "jayandsilentbobreboot",
                    Title = "Jay and Silent Bob Reboot",
                    Image = "jayandsilentbobreboot.jpg",
                    Rating = 2,
                    Description =
                        "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                    Icons = new List<string> {"16", "alcoholdrugabuse", "explicitlanguage"},
                    MovieDetails = "105 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 10, 15),
                    Showtimes = new List<string> {"10:00", "15:00", "20:00"},
                    PricePerTicket = 10D,
                    PopularityScore = 200,
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "riseofskywalker",
                    Title = "Star Wars : The Rise of Skywalker",
                    Image = "riseofskywalker.jpg",
                    Rating = 3,
                    Description =
                        "The surviving Resistance faces the First Order once more in the final chapter of the Skywalker saga.",
                    Icons = new List<string>(),
                    MovieDetails = "152 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 10, 15),
                    Showtimes = new List<string> {"10:00", "13:00", "15:00", "20:00", "23:00"},
                    PricePerTicket = 10D,
                    PopularityScore = 500,
                }
            };
        }

        public static List<Review> GetDefaultReviews()
        {
            var movies = GetDefaultMovies().ToArray();
            var gotId = movies[0].Id;
            var jsbId = movies[1].Id;
            var tros = movies[2].Id;

            return new List<Review>
            {
                new Review() { Id = Guid.Parse("6064db40-59b6-4778-b406-5ed41dc72d92"), MovieIdentifier = gotId, Description = "The last episode sucked!", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Id = Guid.Parse("44d69ef0-a302-47e0-9324-cd32baafa9f1"), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                new Review() { Id = Guid.Parse("46c61dad-fe2f-452e-884e-d189b1d48e0b"), MovieIdentifier = jsbId, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
                new Review() { Id = Guid.Parse("8face9aa-7bb0-403b-bf4e-099bf4e31b34"), MovieIdentifier = jsbId, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
                new Review() { Id = Guid.Parse("bb6982cc-83c5-4d24-b832-14bf68a29fa1"), MovieIdentifier = tros, Description = "Come on JJ, finish this one good!", ReviewedAt = new DateTime(2019, 10, 8, 8, 14, 56) },
                new Review() { Id = Guid.Parse("23faa6e7-b4af-4751-ba5f-e272f84f569c"), MovieIdentifier = tros, Description = "Can't wait for Picard to return to Mordor!", ReviewedAt = new DateTime(2019, 10, 10, 13, 37, 00) },
            };
        }

    }
}
