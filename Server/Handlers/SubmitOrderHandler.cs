using System;
using System.Threading.Tasks;
using System.Web;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Commands;
using Shared.Messages;

namespace Server.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        static ILog log = LogManager.GetLogger<SubmitOrderHandler>();

        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            var immediatelyApproved = true;

            if (message.Movie == 1)
            {
                log.Info("Order for game of thrones at !");
                immediatelyApproved = false;
            }
            else
            {
                log.Info($"Order for a regular movie at {message.Time}.");
            }

            return context.Reply(new OrderSubmission()
            {
                OrderId = Guid.NewGuid(),
                Movie = message.Movie,
                MovieTime = message.Time,
                Theater = message.Theater,
                NumberOfTickets = message.NumberOfTickets,
                Approved = immediatelyApproved,
            });

            // context.SendLocal(someNewMessage);
            // context.Publish(OrderSubmitted);
        }
    }
}
