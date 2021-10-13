using System.Linq;
using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Messages;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using LiteDB;
using Microsoft.Extensions.Logging;
using Shared.Entities;

namespace EventualConsistencyDemo.Handlers
{
    public class OrderSubmissionHandler : IHandleMessages<OrderSubmission>
    {
        IHubContext<TicketHub> ticketHubContext;
        private readonly LiteRepository db;
        readonly ILogger<OrderSubmissionHandler> logger;

        public OrderSubmissionHandler(IHubContext<TicketHub> ticketHubContext, LiteRepository db, ILogger<OrderSubmissionHandler> logger)
        {
            this.ticketHubContext = ticketHubContext;
            this.db = db;
            this.logger = logger;
        }

        public async Task Handle(OrderSubmission message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                logger.LogError("Could not find SignalR ConnectionId from message headers.");
                return;
            }
            
            var movie = db.Query<Movie>().Where(s => s.Id == message.Movie).SingleOrDefault();
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

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);
        }
    }
}
