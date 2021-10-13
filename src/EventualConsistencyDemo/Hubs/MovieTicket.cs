using System;

namespace EventualConsistencyDemo.Hubs
{
    public class MovieTicket
    {
        public string MovieId { get; set; }
        public string TheaterId { get; set; }
        public string Time { get; set; }
        public int NumberOfTickets { get; set; }
    }
}