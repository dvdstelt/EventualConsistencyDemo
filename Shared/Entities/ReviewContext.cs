using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public static class ReviewsContext
    {
        public static List<Review> GetReviews()
        {
            return new List<Review>
            {
                new Review() { Identifier = Guid.Parse("6064db40-59b6-4778-b406-5ed41dc72d92"), MovieIdentifier = 1, Description = "The last episode sucked!", ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12) },
                new Review() { Identifier = Guid.Parse("44d69ef0-a302-47e0-9324-cd32baafa9f1"), MovieIdentifier = 1, Description = "I want more Arya! She's the best!", ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36) },
                new Review() { Identifier = Guid.Parse("46c61dad-fe2f-452e-884e-d189b1d48e0b"), MovieIdentifier = 2, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
                new Review() { Identifier = Guid.Parse("8face9aa-7bb0-403b-bf4e-099bf4e31b34"), MovieIdentifier = 2, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
                new Review() { Identifier = Guid.Parse("bb6982cc-83c5-4d24-b832-14bf68a29fa1"), MovieIdentifier = 3, Description = "Come on JJ, finish this one good!", ReviewedAt = new DateTime(2019, 10, 8, 8, 14, 56) },
                new Review() { Identifier = Guid.Parse("23faa6e7-b4af-4751-ba5f-e272f84f569c"), MovieIdentifier = 3, Description = "Can't wait for Picard to return to Mordor!", ReviewedAt = new DateTime(2019, 10, 10, 13, 37, 00) },
            };
        }
    }
}
