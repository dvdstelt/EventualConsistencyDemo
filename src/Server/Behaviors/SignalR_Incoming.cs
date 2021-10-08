using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace Server.Behaviors
{
    public class SignalR_Incoming : Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            if (context.Message.Headers.TryGetValue("SignalRConnectionId", out string signalRConnectionId))
            {
                // Storing it in NServiceBus context bag, so we can propagate outgoing messages with it as well
                context.Extensions.Set("SignalRConnectionId", signalRConnectionId);
            }

            return next();
        }
    }
}