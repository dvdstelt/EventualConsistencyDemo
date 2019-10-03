using System;
using System.Collections.Generic;

namespace EventualConsistencyDemo.Models
{
    public class TheatersContext
    {
        public static List<Theater> GetTheaters()
        {
            var theaters = new List<Theater>
                {
                    new Theater() { Id = 1, Name = "Rotterdam - De Kuip" },
                    new Theater() { Id = 2, Name = "Rotterdam - Schouwburgplein" },
                    new Theater() { Id = 3, Name = "Den Haag - Spuimarkt" },
                    new Theater() { Id = 4, Name = "Den Haag - Scheveningen" },
                    new Theater() { Id = 5, Name = "Breda" },
                    new Theater() { Id = 6, Name = "Eindhoven" },
                };

            return theaters;
        }
    }
}
