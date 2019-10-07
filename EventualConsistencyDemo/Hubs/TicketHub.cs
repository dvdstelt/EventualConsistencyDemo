using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Commands;

namespace EventualConsistencyDemo.Hubs
{
    public class TicketHub : Hub
    {
        IMessageSession messageSession;

        public TicketHub(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public Task SubmitOrder(int theater, int movie, string time, int numberOfTickets)
        {
            return messageSession.Send(new SubmitOrder() 
            {
                Theater = theater,
                Movie = movie,
                Time = time,
                NumberOfTickets = numberOfTickets
            });
        }
    }
}
