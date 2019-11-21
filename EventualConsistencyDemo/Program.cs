using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared.Configuration;

namespace EventualConsistencyDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);

            // Create the LiteDb database so we can work with some default movies.
            Database.Setup();

            // Configure NServiceBus
            host.UseNServiceBus(hostBuilderContext =>
            {
                var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
                endpointConfiguration.ApplyCommonConfiguration(routingConfig =>
                {
                    routingConfig.RouteToEndpoint(typeof(Shared.Commands.SubmitOrder), "server");
                });

                return endpointConfiguration;
            });

            // Configure web-host.
            host.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
            
            await host.Build().RunAsync();
        }
    }
}
