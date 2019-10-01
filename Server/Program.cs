using System;
using System.Threading.Tasks;
using NServiceBus;
using static Shared.Configuration.Configuration;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = ConfigureEndpoint("server");

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press [ENTER] to quit...");
            Console.ReadKey(true);

            await endpoint.Stop();
        }
    }
}
