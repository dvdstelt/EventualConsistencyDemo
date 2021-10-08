using System.Threading.Tasks;
using LiteDB;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Entities;
using Shared.Events;

namespace Server.Handlers
{
    public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        private readonly LiteRepository db;
        static ILog log = LogManager.GetLogger<OrderAcceptedHandler>();

        public OrderAcceptedHandler(LiteRepository db) => this.db = db;

        public Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            var order = db.Query<Order>()
                .Where(s => s.Id == message.OrderId)
                .SingleOrDefault();

            var movie = db.Query<Movie>()
                .Where(s => s.Id == order.MovieIdentifier)
                .SingleOrDefault();

            // If there's a drawing for tickets, then leave this handler. We don't want to send an email.
            if (movie.TicketType == TicketType.DrawingTicket)
                return Task.CompletedTask;

            // Send email with digital tickets to user
            return Task.CompletedTask;
        }
    }
}
