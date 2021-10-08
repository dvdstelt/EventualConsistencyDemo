using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Commands;

namespace EventualConsistencyDemo.Hubs
{
    public class TicketHub : Hub
    {
        readonly IMessageSession messageSession;

        public TicketHub(IMessageSession messageSession) => this.messageSession = messageSession;

        public Task SubmitOrder(string theater, string movie, string time, int numberOfTickets)
        {
            var userConnectionId = this.Context.ConnectionId;

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("SignalRConnectionId", userConnectionId);

            var order = new SubmitOrder();
            order.Theater = Guid.Parse(theater);
            order.Movie = Guid.Parse(movie);
            order.Time = time;
            order.NumberOfTickets = numberOfTickets;
            order.UserId = Guid.Parse("218d92c4-9c42-4e61-80fa-198b22461f61"); // For now, no other users allowed ;-)
            
            return messageSession.Send(order, sendOptions);
        }
    }
}