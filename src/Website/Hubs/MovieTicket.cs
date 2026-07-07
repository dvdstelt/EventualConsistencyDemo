using System;

namespace EventualConsistencyDemo.Hubs
{
    public class MovieTicket
    {
        public string MovieId { get; set; } = string.Empty;
        public string TheaterId { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int NumberOfTickets { get; set; }
    }
}