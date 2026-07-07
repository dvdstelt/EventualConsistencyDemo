using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Events;

namespace Server.Handlers
{
    [Handler]
    public class TicketsIssuedHandler
    {
        static readonly ILog log = LogManager.GetLogger<TicketsIssuedHandler>();

        public Task Handle(TicketsIssued message, IMessageHandlerContext context)
        {
            // Stand-in for a notifications service: email the digital tickets to the user.
            // It reacts to the event on its own, showing the back-end keeps processing
            // after the website already replied to the visitor.
            log.Info($"Emailing digital tickets for order {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
