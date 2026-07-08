using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Data;
using Shared.Entities;
using Shared.Events;

namespace Server.Handlers
{
    [Handler]
    public class OrderAcceptedHandler(MoviesDbContext db, ILogger<OrderAcceptedHandler> logger)
    {
        public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            var order = await db.Orders
                .SingleOrDefaultAsync(s => s.Id == message.OrderId, context.CancellationToken);

            if (order == null)
            {
                logger.LogWarning("Order {OrderId} not found; skipping.", message.OrderId);
                return;
            }

            var movie = await db.Movies
                .SingleOrDefaultAsync(s => s.Id == order.MovieIdentifier, context.CancellationToken);

            // If there's a drawing for tickets, then leave this handler. We don't want to send an email.
            if (movie == null || movie.TicketType == TicketType.DrawingTicket)
                return;

            // The order is confirmed. Publish an event so the rest of the back-end can react
            // (e.g. email the digital tickets) and keep working even if the user already left.
            await context.Publish(new TicketsIssued { OrderId = order.Id });
        }
    }
}
