using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserIdentifier { get; set; }
        public Guid MovieIdentifier { get; set; }
        public Guid TheaterIdentifier { get; set; }
        public int NumberOfTickets { get; set; }
        public string MovieTime { get; set; }
    }
}
