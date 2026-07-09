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
        public string Background { get; set; } = string.Empty;

        public int Rating { get; set; }
        public List<string> Icons { get; set; } = [];
        public string MovieDetails { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        // When the movie premieres. A future date means it isn't showing yet and
        // is flagged "Soon"; null means it is already in the regular programme.
        public DateTime? PremiereDate { get; set; }

        public double PricePerTicket { get; set; }
        public int PopularityScore { get; set; }
        public TicketType TicketType { get; set; } = TicketType.RegularTicket;
        public MovieCategory Category { get; set; } = MovieCategory.NowShowing;

        public List<string> Showtimes { get; set; } = [];
    }

    public enum TicketType
    {
        RegularTicket,
        DrawingTicket
    }

    // Which homepage section a movie belongs to.
    public enum MovieCategory
    {
        NowShowing,
        SpecialEvent,
        MusicEvent
    }
}
