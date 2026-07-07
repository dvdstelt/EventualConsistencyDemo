using LiteDB;
using Microsoft.Extensions.Logging.Abstractions;
using NServiceBus.Testing;
using Server.Handlers;
using Shared.Entities;
using Shared.Events;
using Xunit;

namespace Tests;

public class OrderAcceptedHandlerTests
{
    static Guid SeedOrder(LiteRepository db, Movie movie)
    {
        var order = new Order { Id = Guid.NewGuid(), MovieIdentifier = movie.Id };
        db.Insert(order);
        return order.Id;
    }

    [Fact]
    public async Task Regular_ticket_publishes_TicketsIssued()
    {
        var movie = TestDatabase.Movie(TicketType.RegularTicket);
        var db = TestDatabase.Create(movie);
        var orderId = SeedOrder(db, movie);
        var context = new TestableMessageHandlerContext();

        await new OrderAcceptedHandler(db, NullLogger<OrderAcceptedHandler>.Instance).Handle(new OrderAccepted { OrderId = orderId }, context);

        var published = Assert.IsType<TicketsIssued>(Assert.Single(context.PublishedMessages).Message);
        Assert.Equal(orderId, published.OrderId);
    }

    [Fact]
    public async Task Drawing_ticket_publishes_nothing()
    {
        var movie = TestDatabase.Movie(TicketType.DrawingTicket);
        var db = TestDatabase.Create(movie);
        var orderId = SeedOrder(db, movie);
        var context = new TestableMessageHandlerContext();

        await new OrderAcceptedHandler(db, NullLogger<OrderAcceptedHandler>.Instance).Handle(new OrderAccepted { OrderId = orderId }, context);

        Assert.Empty(context.PublishedMessages);
    }

    [Fact]
    public async Task Missing_order_is_skipped_without_throwing()
    {
        var db = TestDatabase.Create();
        var context = new TestableMessageHandlerContext();

        await new OrderAcceptedHandler(db, NullLogger<OrderAcceptedHandler>.Instance).Handle(new OrderAccepted { OrderId = Guid.NewGuid() }, context);

        Assert.Empty(context.PublishedMessages);
    }
}
