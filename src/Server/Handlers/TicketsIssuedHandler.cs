using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Events;

namespace Server.Handlers
{
    [Handler]
    public class TicketsIssuedHandler(ILogger<TicketsIssuedHandler> logger)
    {
        public Task Handle(TicketsIssued message, IMessageHandlerContext context)
        {
            // Stand-in for a notifications service: email the digital tickets to the user.
            // It reacts to the event on its own, showing the back-end keeps processing
            // after the website already replied to the visitor.
            logger.LogInformation("Emailing digital tickets for order {OrderId}", message.OrderId);
            return Task.CompletedTask;
        }
    }
}
