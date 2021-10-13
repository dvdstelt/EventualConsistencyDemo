using System;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using NServiceBus;
using Shared.Commands;
using Shared.Entities;

namespace EventualConsistencyDemo.Hubs
{
    public class TicketHub : Hub
    {
        readonly IMessageSession messageSession;
        readonly MovieTickets movieTickets;

        public TicketHub(IMessageSession messageSession, MovieTickets movieTickets)
        {
            this.messageSession = messageSession;
            this.movieTickets = movieTickets;
        }

        public async Task SubmitOrder(MovieTicket ticket)
        {
            var userConnectionId = this.Context.ConnectionId;

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("SignalRConnectionId", userConnectionId);

            var order = new SubmitOrder
            {
                Theater = Guid.Parse(ticket.TheaterId),
                Movie = Guid.Parse(ticket.MovieId),
                Time = ticket.Time,
                NumberOfTickets = ticket.NumberOfTickets,
                UserId = Guid.Parse("218d92c4-9c42-4e61-80fa-198b22461f61") // For now, no other users allowed ;-)
            };

            await messageSession.Send(order, sendOptions).ConfigureAwait(false);

            await movieTickets.ReportOnLottery(ticket, Context.ConnectionId);
        }
    }
}