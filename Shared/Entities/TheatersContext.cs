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
                    new Theater() { Id = Guid.Parse("eebe0187-f1eb-4971-a909-af3e77381a62"), Name = "Rotterdam - De Kuip" },
                    new Theater() { Id = Guid.Parse("bb5450d1-675c-478e-9945-fe789ad5e202"), Name = "Rotterdam - Schouwburgplein" },
                    new Theater() { Id = Guid.Parse("511b9a15-3dec-4ec8-afd2-32c1ac3bbb63"), Name = "Den Haag - Spuimarkt" },
                    new Theater() { Id = Guid.Parse("65801707-17fc-4acd-b6a5-3f210f718fcc"), Name = "Den Haag - Scheveningen" },
                    new Theater() { Id = Guid.Parse("308617eb-49c3-425f-82b6-97896683bd3b"), Name = "Breda" },
                    new Theater() { Id = Guid.Parse("0e885038-5a2b-486f-b1fa-187bec2c9d96"), Name = "Eindhoven" },
                };

            return theaters;
        }
    }
}
