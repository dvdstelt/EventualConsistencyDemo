using System.Linq;
using Shared.Data;
using Shared.Entities;
using Xunit;

namespace Tests;

public class DatabaseSeedTests
{
    [Fact]
    public void Seed_loads_movies_and_reviews()
    {
        var db = TestDatabase.Create();  // empty in-memory database, schema created from the model
        SeedData.Seed(db);

        Assert.Equal(21, db.Movies.Count());  // tracks the SeedData catalogue
        Assert.Equal(6, db.Reviews.Count());

        var got = db.Movies.Single(m => m.UrlTitle == "gameofthrones");
        Assert.Equal(TicketType.DrawingTicket, got.TicketType);   // enum round-trips as text
        Assert.Contains("violence", got.Icons);                    // JSON collection round-trips
        Assert.Equal(2, db.Reviews.Count(r => r.MovieIdentifier == got.Id)); // seeded ids match EF's

        var titanic = db.Movies.Single(m => m.UrlTitle == "titanic");
        Assert.Equal(MovieCategory.SpecialEvent, titanic.Category); // Category enum round-trips as text

        Assert.Equal(new DateTime(2026, 8, 15), db.Movies.Single(m => m.UrlTitle == "toy-story-5").ReleaseDate); // upcoming
    }

    [Fact]
    public void Seed_is_idempotent()
    {
        var db = TestDatabase.Create();
        SeedData.Seed(db);
        SeedData.Seed(db);

        Assert.Equal(21, db.Movies.Count());
    }
}
