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
                    Id = Guid.Parse("daa1bd7d-af4c-4581-b25f-e0f21a8ba22b"),
                    UrlTitle = "gameofthrones",
                    Title = "Game of Thrones",
                    Image = "gameofthrones.jpg",
                    Rating = 5,
                    Description = "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                    Icons = new List<string> { "16", "sex", "violence" },
                    MovieDetails = "80 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 05, 19),
                    Showtimes = new List<string> { "19:00" },
                    PricePerTicket = 0D,
                    PopularityScore = 1000,
                    TicketType = TicketType.DrawingTicket,
                },
                new Movie
                {
                    Id = Guid.Parse("0704d943-beee-4999-ba1e-c73489ead6ec"),
                    UrlTitle = "jayandsilentbobreboot",
                    Title = "Jay and Silent Bob Reboot",
                    Image = "jayandsilentbobreboot.jpg",
                    Rating = 2,
                    Description = "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                    Icons = new List<string> { "16", "alcoholdrugabuse", "explicitlanguage" },
                    MovieDetails = "105 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 10, 15),
                    Showtimes = new List<string> { "10:00", "15:00", "20:00" },
                    PricePerTicket = 10D,
                    PopularityScore = 800,
                    TicketType = TicketType.RegularTicket,
                },
                new Movie
                {
                    Id = Guid.Parse("ca4245ca-7d12-44ea-a421-04115414897b"),
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
                    PopularityScore = 600,
                    TicketType = TicketType.RegularTicket,
                }
            };
        }
    }

}
