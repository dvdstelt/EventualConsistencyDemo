using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class MoviesContext
    {
        public static List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie
                {
                    Id = 1,
                    UrlTitle = "gameofthrones",
                    Title = "Game of Thrones",
                    Image = "got.jpg",
                    Rating = 5,
                    Description = "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                    Icons = new List<string> { "16", "sex", "violence" },
                    MovieDetails = "80 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 05, 19),
                    Showtimes = new List<string> { "19:00" },
                    PricePerTicket = 0D,
                },
                new Movie
                {
                    Id = 2,
                    UrlTitle = "jayandsilentbobreboot",
                    Title = "Jay and Silent Bob Reboot",
                    Image = "jaysilentbobreboot.jpg",
                    Rating = 2,
                    Description = "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                    Icons = new List<string> { "16", "alcoholdrugabuse", "explicitlanguage" },
                    MovieDetails = "105 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 10, 15),
                    Showtimes = new List<string> { "10:00", "15:00", "20:00" },
                    PricePerTicket = 10D,
                },
                new Movie
                {
                    Id = 3,
                    UrlTitle = "riseofskywalker",
                    Title = "Star Wars : The Rise of Skywalker",
                    Image = "riseofskywalker.jpg",
                    Rating = 3,
                    Description = "The surviving Resistance faces the First Order once more in the final chapter of the Skywalker saga.",
                    Icons = new List<string>(),
                    MovieDetails = "152 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 10, 15),
                    Showtimes = new List<string> { "10:00", "13:00", "15:00", "20:00", "23:00" },
                    PricePerTicket = 10D,
                }
            };
        }
    }

}
