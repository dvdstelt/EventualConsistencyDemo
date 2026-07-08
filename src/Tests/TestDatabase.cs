using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace Tests;

// An in-memory SQLite MoviesDbContext, optionally seeded with the given movies.
// The connection stays open for the lifetime of the context so the in-memory
// database survives across calls.
static class TestDatabase
{
    public static MoviesDbContext Create(params Movie[] movies)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<MoviesDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new MoviesDbContext(options);
        db.Database.EnsureCreated();

        if (movies.Length > 0)
        {
            db.Movies.AddRange(movies);
            db.SaveChanges();
        }

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
