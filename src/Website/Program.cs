using EventualConsistencyDemo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared.Configuration;


var host = Host.CreateDefaultBuilder(args)
    // NServiceBus needs to be configured first!
    .UseNServiceBus(hostBuilderContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
        endpointConfiguration.ApplyCommonConfiguration(routingConfig =>
        {
            routingConfig.RouteToEndpoint(typeof(Shared.Commands.SubmitOrder), "server");
        });

        return endpointConfiguration;
    })
    .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
    .Build();

// Create the LiteDb database so we can work with some default movies.
Database.Setup();

await host.RunAsync();