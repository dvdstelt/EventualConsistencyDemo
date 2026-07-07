using System;
using System.Collections.Generic;

namespace Shared.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string UrlTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public int Rating { get; set; }
        public List<string> Icons { get; set; } = [];
        public string MovieDetails { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public double PricePerTicket { get; set; }
        public int PopularityScore { get; set; }
        public TicketType TicketType { get; set; } = TicketType.RegularTicket;

        public List<string> Showtimes { get; set; } = [];
    }

    public enum TicketType
    {
        RegularTicket,
        DrawingTicket
    }
}
