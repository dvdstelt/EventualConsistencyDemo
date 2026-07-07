using Microsoft.Extensions.Logging.Abstractions;
using NServiceBus.Testing;
using Server.Handlers;
using Shared.Commands;
using Shared.Entities;
using Shared.Events;
using Shared.Messages;
using Xunit;

namespace Tests;

public class SubmitOrderHandlerTests
{
    static SubmitOrder OrderFor(Movie movie) => new()
    {
        UserId = Guid.NewGuid(),
        Movie = movie.Id,
        Theater = Guid.NewGuid(),
        Time = "20:00",
        NumberOfTickets = 2
    };

    [Fact]
    public async Task Regular_ticket_is_approved_immediately()
    {
        var movie = TestDatabase.Movie(TicketType.RegularTicket);
        var db = TestDatabase.Create(movie);
        var context = new TestableMessageHandlerContext();

        await new SubmitOrderHandler(db, NullLogger<SubmitOrderHandler>.Instance).Handle(OrderFor(movie), context);

        var reply = Assert.IsType<OrderSubmission>(Assert.Single(context.RepliedMessages).Message);
        Assert.True(reply.Approved);
    }

    [Fact]
    public async Task Drawing_ticket_is_not_approved_immediately()
    {
        var movie = TestDatabase.Movie(TicketType.DrawingTicket);
        var db = TestDatabase.Create(movie);
        var context = new TestableMessageHandlerContext();

        await new SubmitOrderHandler(db, NullLogger<SubmitOrderHandler>.Instance).Handle(OrderFor(movie), context);

        var reply = Assert.IsType<OrderSubmission>(Assert.Single(context.RepliedMessages).Message);
        Assert.False(reply.Approved);
    }

    [Fact]
    public async Task Order_is_persisted_before_OrderAccepted_is_published()
    {
        var movie = TestDatabase.Movie(TicketType.RegularTicket);
        var db = TestDatabase.Create(movie);
        var context = new TestableMessageHandlerContext();

        await new SubmitOrderHandler(db, NullLogger<SubmitOrderHandler>.Instance).Handle(OrderFor(movie), context);

        // The persisted order must be the one referenced by the published event, so the
        // subscriber can look it up (guards the ordering fix).
        var order = Assert.Single(db.Query<Order>().ToList());
        var published = Assert.IsType<OrderAccepted>(Assert.Single(context.PublishedMessages).Message);
        Assert.Equal(order.Id, published.OrderId);
    }

    [Fact]
    public async Task Unknown_movie_throws()
    {
        var db = TestDatabase.Create();
        var context = new TestableMessageHandlerContext();
        var command = new SubmitOrder { Movie = Guid.NewGuid(), Time = "20:00", NumberOfTickets = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() => new SubmitOrderHandler(db, NullLogger<SubmitOrderHandler>.Instance).Handle(command, context));
    }
}
