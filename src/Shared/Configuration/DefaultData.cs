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
                //new Movie
                //{
                //    Id = Guid.NewGuid(),
                //    UrlTitle = "jayandsilentbobreboot",
                //    Title = "Jay and Silent Bob Reboot",
                //    Image = "jayandsilentbobreboot.jpg",
                //    Rating = 2,
                //    Description =
                //        "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                //    Icons = new List<string> {"16", "alcoholdrugabuse", "explicitlanguage"},
                //    MovieDetails = "105 minutes | English, Dutch subtitles",
                //    ReleaseDate = new DateTime(2019, 10, 15),
                //    Showtimes = new List<string> {"10:00", "15:00", "20:00"},
                //    PricePerTicket = 10D,
                //    PopularityScore = 200,
                //},
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
                    PricePerTicket = 10D,
                    PopularityScore = 500,
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "freeguy",
                    Title = "FreeGuy",
                    Image = "freeguy.jpg",
                    Rating = 5,
                    Description = "A bank teller discovers that he's actually an NPC inside a brutal, open world video game.",
                    Icons = new List<string> {"12", "violence", "explicitlanguage" },
                    MovieDetails = "115 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2021,08,11),
                    Showtimes = new List<string> { "15:00", "20:00", "23:00" },
                    PricePerTicket = 20D,
                    PopularityScore = 500
                }
            };
        }

        public static List<Review> GetDefaultReviews(IEnumerable<Movie> movies)
        {
            var gotId = movies.Single(s => s.UrlTitle == "gameofthrones").Id;
            //var jsbId = movies.Single(s => s.UrlTitle == "jayandsilentbobreboot").Id;
            // var tros = movies.Single(s => s.UrlTitle == "riseofskywalker").Id;
            // var tenet = movies.Single(s => s.UrlTitle == "tenet").Id;
            var dune = movies.Single(s => s.UrlTitle == "dune").Id;
            var freeguy = movies.Single(s => s.UrlTitle == "freeguy").Id;

            return new List<Review>
            {
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "The last episode sucked!", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Great visuals, screenplay adaptation is too close to the books though.", ReviewedAt = new DateTime(2021, 10, 8, 8, 14, 56) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Can't wait to go back to Mordor!", ReviewedAt = new DateTime(2021, 10, 9, 13, 37, 00) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = freeguy, Description = "I love Reynolds his humor, this movie is epic!", ReviewedAt = new DateTime(2021, 09, 21, 12, 12, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = freeguy, Description = "May the force be with you, always!", ReviewedAt = new DateTime(2021, 09, 20, 13, 45, 18) },
            };
        }

    }
}
