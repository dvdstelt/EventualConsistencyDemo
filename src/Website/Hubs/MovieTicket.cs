namespace EventualConsistencyDemo.Hubs
{
    public sealed record MovieTicket
    {
        public string MovieId { get; init; } = string.Empty;
        public string TheaterId { get; init; } = string.Empty;
        public string Time { get; init; } = string.Empty;
        public int NumberOfTickets { get; init; }
    }
}