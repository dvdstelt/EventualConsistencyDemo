using System;
using System.Linq;
using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Messages;
using System.Threading.Tasks;
using System.Web;
using EventualConsistencyDemo.Models;
using Shared.Entities;

namespace EventualConsistencyDemo.Handlers
{
    public class OrderSubmissionHandler : IHandleMessages<OrderSubmission>
    {
        IHubContext<TicketHub> ticketHubContext;

        public OrderSubmissionHandler(IHubContext<TicketHub> ticketHubContext)
        {
            this.ticketHubContext = ticketHubContext;
        }

        public Task Handle(OrderSubmission message, IMessageHandlerContext context)
        {
            var movie = MoviesContext.GetMovies().Single(s => s.Id == message.Movie);
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

            return ticketHubContext.Clients.All.SendAsync("OrderSubmission", screenMessage);
        }
    }
}
