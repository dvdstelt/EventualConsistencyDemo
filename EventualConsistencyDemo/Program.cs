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
            host.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

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

            await host.Build().RunAsync();
        }
    }
}
