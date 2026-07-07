using System;

namespace Shared.Events
{
    public sealed record OrderAccepted
    {
        public Guid OrderId { get; init; }
    }
}
