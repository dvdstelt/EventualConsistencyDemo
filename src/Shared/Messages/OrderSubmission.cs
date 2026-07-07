using System;

namespace Shared.Messages
{
    public class OrderSubmission
    {
        public Guid OrderId { get; set; }
        public Guid Movie { get; set; }
        public string MovieTime { get; set; } = string.Empty;
        public Guid Theater { get; set; }
        public bool Approved { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
