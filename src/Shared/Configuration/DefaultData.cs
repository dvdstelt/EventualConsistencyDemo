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
                },
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "tenet",
                    Title = "Tenet",
                    Image = "tenet.jpg",
                    Rating = 4,
                    Description = "Armed with only one word, Tenet, and fighting for the survival of the entire world, a Protagonist journeys through a twilight world of international espionage on a mission that will unfold in something beyond real time.",
                    Icons = new List<string> {"12", "fear", "violence", "explicitlanguage" },
                    MovieDetails = "150 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2020,08,27),
                    Showtimes = new List<string> { "10:00","13:00","15:00", "20:00", "23:00"},
                    PricePerTicket = 20D,
                    PopularityScore = 500
                }
            };
        }

        public static List<Review> GetDefaultReviews(IEnumerable<Movie> movies)
        {
            var gotId = movies.Single(s => s.UrlTitle == "gameofthrones").Id;
            //var jsbId = movies.Single(s => s.UrlTitle == "jayandsilentbobreboot").Id;
            var tros = movies.Single(s => s.UrlTitle == "riseofskywalker").Id;
            var tenet = movies.Single(s => s.UrlTitle == "tenet").Id;

            return new List<Review>
            {
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "The last episode sucked!", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = tros, Description = "Come on JJ, finish this one good!", ReviewedAt = new DateTime(2019, 10, 8, 8, 14, 56) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = tros, Description = "Can't wait for Picard to return to Mordor!", ReviewedAt = new DateTime(2019, 10, 10, 13, 37, 00) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = tenet, Description = "Mind is blown! I need to see this again!", ReviewedAt = new DateTime(2020, 07, 8, 12, 12, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = tenet, Description = "I didn't get it", ReviewedAt = new DateTime(2020, 07, 9, 13, 45, 18) },
            };
        }

    }
}
