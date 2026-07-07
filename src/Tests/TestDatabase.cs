using LiteDB;
using Shared.Entities;

namespace Tests;

// An in-memory LiteDB repository, seeded with the given movies. Handlers use
// db.Query<Movie>()/db.Insert(...), which resolve to the same collection here.
static class TestDatabase
{
    public static LiteRepository Create(params Movie[] movies)
    {
        var db = new LiteRepository(new MemoryStream());
        foreach (var movie in movies)
            db.Insert(movie);
        return db;
    }

    public static Movie Movie(TicketType ticketType) => new()
    {
        Id = Guid.NewGuid(),
        UrlTitle = "movie",
        Title = "Movie",
        TicketType = ticketType
    };
}
