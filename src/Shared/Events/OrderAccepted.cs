using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events
{
    public class OrderAccepted
    {
        public OrderAccepted(Guid orderIdentifier)
        {
            this.OrderId = orderIdentifier;
        }

        public Guid OrderId { get; set; }
    }
}
