using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Commands;

namespace Server.Handlers
{
    public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
        public Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            if (message.Movie == 1)
            {
                Console.WriteLine("Order for game of thrones!");
            }
            else
            {
                Console.WriteLine("Order for a regular movie.");
            }

            return Task.CompletedTask;
        }
    }
}
