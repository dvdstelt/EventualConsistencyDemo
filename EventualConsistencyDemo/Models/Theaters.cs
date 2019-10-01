using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace EventualConsistencyDemo.Models
{
    public class Theaters
    {
        private readonly IMemoryCache memoryCache;

        public Theaters(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public List<Theater> GetTheaters()
        {
            if (!memoryCache.TryGetValue("Theaters", out List<Theater> theaters))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15));

                theaters = new List<Theater>
                {
                    new Theater() { Id = 1, Name = "Rotterdam - De Kuip" },
                    new Theater() { Id = 2, Name = "Rotterdam - Schouwburgplein" },
                    new Theater() { Id = 3, Name = "Den Haag - Spuimarkt" },
                    new Theater() { Id = 4, Name = "Den Haag - Scheveningen" },
                    new Theater() { Id = 5, Name = "Breda" },
                    new Theater() { Id = 6, Name = "Eindhoven" },
                };

                memoryCache.Set("Theaters", theaters, cacheEntryOptions);
            }

            return theaters;
        }
    }
}
