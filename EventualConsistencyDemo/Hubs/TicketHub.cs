using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;

namespace EventualConsistencyDemo.Hubs
{
    public class TicketHub : Hub
    {
        IMessageSession messageSession;

        public TicketHub(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
