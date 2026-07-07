using System;

namespace Shared.Messages
{
    public sealed record OrderSubmission
    {
        public Guid OrderId { get; init; }
        public Guid Movie { get; init; }
        public string MovieTime { get; init; } = string.Empty;
        public Guid Theater { get; init; }
        public bool Approved { get; init; }
        public int NumberOfTickets { get; init; }
    }
}
