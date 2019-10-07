using System;

namespace Shared.Messages
{
    public class OrderSubmission
    {
        public Guid OrderId { get; set; }
        public int Movie { get; set; }
        public string MovieTime { get; set; }
        public int Theater { get; set; }
        public bool Approved { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
