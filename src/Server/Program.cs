using System;
using System.Threading.Tasks;
using LiteDB;
using NServiceBus;
using Server.Behaviors;
using Shared.Configuration;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("server").ApplyCommonConfiguration();

            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new SignalR_Incoming(), "Stores SignalR user identifier into context.");
            pipeline.Register(new SignalR_Outgoing(), "Propagates SignalR user identifier to outgoing messages.");            
            
            endpointConfiguration.RegisterComponents(s =>
            {
                s.ConfigureComponent(() => new LiteRepository(Database.DatabaseConnectionstring), 
                    DependencyLifecycle.InstancePerUnitOfWork);
            });

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press [ENTER] to quit...");
            Console.ReadKey(true);

            await endpoint.Stop();
        }
    }
}
