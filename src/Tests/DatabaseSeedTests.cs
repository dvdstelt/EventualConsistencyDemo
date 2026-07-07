using System.Linq;
using Shared.Data;
using Shared.Entities;
using Xunit;

namespace Tests;

public class DatabaseSeedTests
{
    [Fact]
    public void Seed_script_loads_movies_and_reviews()
    {
        var db = TestDatabase.Create();      // empty in-memory database, schema created from the model
        DatabaseInitializer.Initialize(db);  // runs Seed.sql

        Assert.Equal(3, db.Movies.Count());
        Assert.Equal(6, db.Reviews.Count());

        var got = db.Movies.Single(m => m.UrlTitle == "gameofthrones");
        Assert.Equal(TicketType.DrawingTicket, got.TicketType);   // enum round-trips as text
        Assert.Contains("violence", got.Icons);                    // JSON collection round-trips
        Assert.Equal(2, db.Reviews.Count(r => r.MovieIdentifier == got.Id)); // seeded ids match EF's
    }

    [Fact]
    public void Initialize_is_idempotent()
    {
        var db = TestDatabase.Create();
        DatabaseInitializer.Initialize(db);
        DatabaseInitializer.Initialize(db);

        Assert.Equal(3, db.Movies.Count());
    }
}
