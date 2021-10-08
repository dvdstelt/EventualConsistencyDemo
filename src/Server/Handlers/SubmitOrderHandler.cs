using System;
using System.Threading.Tasks;
using LiteDB;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Commands;
using Shared.Entities;
using Shared.Events;
using Shared.Messages;

namespace Server.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        static readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();
        
        private readonly LiteRepository db;

        public SubmitOrderHandler(LiteRepository db) => this.db = db;

        public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order arrived for movie {message.Movie}");

            var movie = db.Query<Movie>()
                .Where(s => s.Id == message.Movie)
                .SingleOrDefault();

            if (movie == null)
                throw new ArgumentException($"Movie {message.Movie} not found in datastore", nameof(message.Movie));

            var order = new Order
            {
                Id = Guid.NewGuid(),
                MovieIdentifier = message.Movie,
                TheaterIdentifier = message.Theater,
                UserIdentifier = message.UserId,
                MovieTime = message.Time,
                NumberOfTickets = message.NumberOfTickets
            };

            bool immediatelyApproved = movie.TicketType != TicketType.DrawingTicket;

            await context.Reply(new OrderSubmission()
            {
                OrderId = Guid.NewGuid(),
                Movie = message.Movie,
                MovieTime = message.Time,
                Theater = message.Theater,
                NumberOfTickets = message.NumberOfTickets,
                Approved = immediatelyApproved,
            });

            // We could prevent publishing if it's a TicketType.DrawingTicket.
            // But what if in the future we do want to get notified? We'd have to change this handler again.
            // So let's put it in right now! :-)
            await context.Publish(new OrderAccepted(order.Id));

            // Insert last because it's a non-transactional resource that cannot easily be rolled back.
            db.Insert(order);
        }
    }
}
