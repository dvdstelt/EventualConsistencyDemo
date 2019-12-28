using System;

namespace Shared.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid MovieIdentifier { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string Description { get; set; }
        public string PostedByEmail { get; set; }
    }
}
