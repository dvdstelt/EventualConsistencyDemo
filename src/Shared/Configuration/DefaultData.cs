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
                    PricePerTicket = 20D,
                    PopularityScore = 500,
                },
                // new Movie
                // {
                //     Id = Guid.NewGuid(),
                //     UrlTitle = "freeguy",
                //     Title = "FreeGuy",
                //     Image = "freeguy.jpg",
                //     Rating = 5,
                //     Description = "A bank teller discovers that he's actually an NPC inside a brutal, open world video game.",
                //     Icons = new List<string> {"12", "violence", "explicitlanguage" },
                //     MovieDetails = "115 minutes | English, Dutch subtitles",
                //     ReleaseDate = new DateTime(2021,08,11),
                //     Showtimes = new List<string> { "15:00", "20:00", "23:00" },
                //     PricePerTicket = 10D,
                //     PopularityScore = 250
                // }
                new Movie
                {
                    Id = Guid.NewGuid(),
                    UrlTitle = "maverick",
                    Title = "Top Gun Maverick",
                    Image = "maverick.jpg",
                    Rating = 5,
                    Description = "After thirty years, Maverick is still pushing the envelope as a top naval aviator, but must confront ghosts of his past when he leads TOP GUN's elite graduates on a mission that demands the ultimate sacrifice from those chosen to fly it.",
                    Icons = new List<string> {"12", "violence", "explicitlanguage" },
                    MovieDetails = "2 hours 10 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2022,04,27),
                    Showtimes = new List<string> { "15:00", "20:00", "23:00" },
                    PricePerTicket = 10D,
                    PopularityScore = 250
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
            // var freeguy = movies.Single(s => s.UrlTitle == "freeguy").Id;
            var maverick = movies.Single(s => s.UrlTitle == "maverick").Id;

            return new List<Review>
            {
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "A riddle: A father and his son fight in the Battle of the Bastards. The father is killed in battle and the son is brought to the Warden of the North to be knighted. The Warden of the North looks at the boy and says: \"I can't knight him, he's my son!\" How can this be?", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
                //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Great visuals, screenplay adaptation is too close to the books though.", ReviewedAt = new DateTime(2021, 10, 8, 8, 14, 56) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = dune, Description = "Can't wait to go back to Mordor!", ReviewedAt = new DateTime(2021, 10, 9, 13, 37, 00) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = maverick, Description = "This movie needs to been in IMAX! That sound is incredible!", ReviewedAt = new DateTime(2022, 09, 21, 12, 12, 12) },
                new Review() { Id = Guid.NewGuid(), MovieIdentifier = maverick, Description = "Maverick is so much better than the original!", ReviewedAt = new DateTime(2022, 09, 20, 13, 45, 18) },
            };
        }

    }
}
