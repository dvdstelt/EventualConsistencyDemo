using System;
using System.IO;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Server.Behaviors;
using Server.Handlers;
using Shared.Configuration;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("NServiceBus", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}",
        theme: SystemConsoleTheme.Literate)
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        var endpointConfiguration = new EndpointConfiguration("server").ApplyCommonConfiguration();

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new SignalR_Incoming(), "Stores SignalR user identifier into context.");
        pipeline.Register(new SignalR_Outgoing(), "Propagates SignalR user identifier to outgoing messages.");

        endpointConfiguration.AddHandler<SubmitOrderHandler>();
        endpointConfiguration.AddHandler<OrderAcceptedHandler>();
        endpointConfiguration.AddHandler<TicketsIssuedHandler>();

        services.AddScoped(_ => new LiteRepository(Database.DatabaseConnectionstring));
        services.AddNServiceBusEndpoint(endpointConfiguration, endpointConfiguration.EndpointName);
    })
    .Build();

var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
try
{
    // Setting the title is not supported when the output is redirected (e.g. a container or CI).
    Console.Title = hostEnvironment.ApplicationName;
}
catch (IOException)
{
}

Log.Information("Press CTRL+C to quit the application...");

host.Run();

Log.Information("Application shut down...");
Log.CloseAndFlush();
