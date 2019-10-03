using System;

namespace Shared.Messages
{
    public class OrderSubmission
    {
        public Guid OrderId { get; set; }
        public int Movie { get; set; }
        public bool ImmediateOrder { get; set; }
    }
}
