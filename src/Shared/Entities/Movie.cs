using System;
using System.Collections.Generic;

namespace Shared.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string UrlTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public int Rating { get; set; }
        public List<string> Icons { get; set; }
        public string MovieDetails { get; set; }
        public DateTime ReleaseDate { get; set; }
        
        public double PricePerTicket { get; set; }
        public int PopularityScore { get; set; }
        public TicketType TicketType { get; set; } = TicketType.RegularTicket;

        public List<string> Showtimes { get; set; }
    }

    public enum TicketType
    {
        RegularTicket,
        DrawingTicket,
        MorningTicket
    }
}
