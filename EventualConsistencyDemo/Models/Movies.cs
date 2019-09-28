using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EventualConsistencyDemo.Models
{
    public class Movies
    {
        private readonly IMemoryCache memoryCache;

        public Movies(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public List<Movie> GetMovies()
        {
            if (!memoryCache.TryGetValue("Movies", out List<Movie> movies))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15));

                movies = new List<Movie>
                {
                    new Movie
                    {
                        Id = 1,
                        UrlTitle = "gameofthrones",
                        Title = "Game of Thrones",
                        Image = "got.jpg",
                        Rating = 5,
                        Description = "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                        Icons = new List<string> { "16", "sex", "violence" }
                    },
                    new Movie
                    {
                        Id = 2,
                        UrlTitle = "jayandsilentbobreboot",
                        Title = "Jay and Silent Bob Reboot",
                        Image = "jaysilentbobreboot.jpg",
                        Rating = 2,
                        Description = "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                        Icons = new List<string> { "16", "alcoholdrugabuse", "explicitlanguage" }
                    },
                    new Movie
                    {
                        Id = 3,
                        UrlTitle = "riseofskywalker",
                        Title = "Star Wars : The Rise of Skywalker",
                        Image = "riseofskywalker.jpg",
                        Rating = 3,
                        Description = "The surviving Resistance faces the First Order once more in the final chapter of the Skywalker saga.",
                    }
                };

                memoryCache.Set("Movies", movies, cacheEntryOptions);
            }

            return movies;
        }
    }
}
