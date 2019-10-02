using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Shared.Messages;
using System.Threading.Tasks;

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
            return ticketHubContext.Clients.All.SendAsync("OrderSubmission", message.OrderId);
        }
    }
}
