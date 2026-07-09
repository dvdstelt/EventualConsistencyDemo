using System;

namespace Shared.Entities
{
    public class Theater
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // The city is the part of the name before " - " ("Rotterdam - De Kuip" -> "Rotterdam").
        // Theaters named after just their city (e.g. "Breda") are their own city.
        public string City => Name.Split(" - ", 2)[0].Trim();
    }
}
