using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared.Configuration;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("server").ApplyCommonConfiguration();

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press [ENTER] to quit...");
            Console.ReadKey(true);

            await endpoint.Stop();
        }
    }
}
