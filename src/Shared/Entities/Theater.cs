using System;

namespace Shared.Entities
{
    public class Theater
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Names are "City - Cinema" ("Rotterdam - De Kuip"); theaters named after
        // just their city (e.g. "Breda") are their own city and cinema.
        public string City
        {
            get
            {
                var i = Name.IndexOf(" - ", StringComparison.Ordinal);
                return i < 0 ? Name : Name[..i].Trim();
            }
        }

        public string CinemaName
        {
            get
            {
                var i = Name.IndexOf(" - ", StringComparison.Ordinal);
                return i < 0 ? Name : Name[(i + 3)..].Trim();
            }
        }
    }
}
