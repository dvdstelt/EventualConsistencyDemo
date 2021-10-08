using System;
using System.Linq;
using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Messages;
using System.Threading.Tasks;
using System.Web;
using EventualConsistencyDemo.Models;
using LiteDB;
using NServiceBus.Logging;
using Shared.Entities;

namespace EventualConsistencyDemo.Handlers
{
    public class OrderSubmissionHandler : IHandleMessages<OrderSubmission>
    {
        static ILog log = LogManager.GetLogger<OrderSubmissionHandler>();

        IHubContext<TicketHub> ticketHubContext;
        private readonly LiteRepository db;

        public OrderSubmissionHandler(IHubContext<TicketHub> ticketHubContext, LiteRepository db)
        {
            this.ticketHubContext = ticketHubContext;
            this.db = db;
        }

        public Task Handle(OrderSubmission message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                log.Error("Could not find SignalR ConnectionId from message headers.");
                return Task.CompletedTask;
            }
            
            var movie = db.Query<Movie>().Where(s => s.Id == message.Movie).SingleOrDefault();
            var theater = TheatersContext.GetTheaters().Single(s => s.Id == message.Theater);

            var screenMessage = "Thank you for your order.<br /><br />";
            screenMessage += "<table>";
            screenMessage += $"<tr><td><b>Movie</b></td><td>: {movie.Title}</td></tr>";
            screenMessage += $"<tr><td><b>Theater</b></td><td>: {theater.Name}</td></tr>";
            screenMessage += $"<tr><td><b>Time</b></td><td>: {message.MovieTime}</td></tr>";
            screenMessage += $"<tr><td><b>Tickets</b></td><td>: {message.NumberOfTickets}</td></tr>";
            screenMessage += "</tr></table><br /><br />";

            if (message.Approved)
            {
                screenMessage +=
                    $"Your order will soon arrive in your email.";
            }
            else
            {
                // For simplicity, it's always 2 weeks in advance.
                screenMessage += $"On <b>{DateTime.Now.AddDays(14):M}</b>, you'll receive an email and hear if you have been selected.";
            }

            var returnMessage = new
            {
                MovieTitle = movie.Title
            };

            return ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderSubmission", screenMessage);
        }
    }
}
