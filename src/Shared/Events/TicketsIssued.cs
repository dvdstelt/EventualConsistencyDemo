using System;

namespace Shared.Events
{
    public sealed record TicketsIssued
    {
        public Guid OrderId { get; init; }
    }
}
