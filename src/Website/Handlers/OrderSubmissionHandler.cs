using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Data;
using Shared.Entities;
using Shared.Messages;

namespace EventualConsistencyDemo.Handlers
{
    [Handler]
    public class OrderSubmissionHandler(
        IHubContext<TicketHub> ticketHubContext,
        MoviesDbContext db,
        ILogger<OrderSubmissionHandler> logger)
    {
        public async Task Handle(OrderSubmission message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                logger.LogError("Could not find SignalR ConnectionId from message headers.");
                return;
            }
            
            var movie = await db.Movies.SingleOrDefaultAsync(s => s.Id == message.Movie, context.CancellationToken);
            if (movie == null)
            {
                logger.LogWarning("Movie {MovieId} not found; skipping.", message.Movie);
                return;
            }

            var theater = TheatersContext.GetTheaters().Single(s => s.Id == message.Theater);

            if (movie.TicketType == TicketType.DrawingTicket)
                return;

            var ticket = new
            {
                TheaterId = theater.Id.ToString(),
                Theater = theater.Name,
                MovieId = movie.Id.ToString(),
                MovieTitle = movie.Title,
                Time = message.MovieTime,
                NumberOfTickets = message.NumberOfTickets
            };

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket, context.CancellationToken);
        }
    }
}
