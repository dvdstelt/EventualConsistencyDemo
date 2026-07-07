using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events
{
    public class OrderAccepted
    {
        public Guid OrderId { get; init; }
    }
}
