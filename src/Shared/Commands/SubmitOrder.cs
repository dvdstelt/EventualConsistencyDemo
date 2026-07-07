using System;

namespace Shared.Commands
{
    public sealed record SubmitOrder
    {
        public Guid UserId { get; init; }
        public Guid Movie { get; init; }
        public Guid Theater { get; init; }
        public string Time { get; init; } = string.Empty;
        public int NumberOfTickets { get; init; }
    }
}
