using System;
using EventualConsistencyDemo.Handlers;
using EventualConsistencyDemo.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Shared.Configuration;
using Shared.Data;

var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
endpointConfiguration.ApplyCommonConfiguration(routingConfig =>
{
    routingConfig.RouteToEndpoint(typeof(Shared.Commands.SubmitOrder), "server");
});
endpointConfiguration.AddHandler<OrderSubmissionHandler>();

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddNServiceBusEndpoint(endpointConfiguration, endpointConfiguration.EndpointName);

// AddNServiceBusEndpoint registers IMessageSession as a keyed singleton (keyed by the
// endpoint name) so multiple endpoints can be hosted side by side. This single-endpoint
// app injects a plain IMessageSession into TicketHub, so expose the keyed session as the default.
services.AddSingleton<IMessageSession>(sp =>
    sp.GetRequiredKeyedService<IMessageSession>(endpointConfiguration.EndpointName));

services.AddControllersWithViews();

services.AddSignalR(o => o.EnableDetailedErrors = true);

services.AddDbContext<MoviesDbContext>(o => o.UseSqlite(Database.ConnectionString));
services.AddScoped<MovieTickets>();
services.AddSingleton(TimeProvider.System);

services.AddMemoryCache();

var app = builder.Build();

// Create the SQLite database from the entity model and seed it from Seed.sql.
using (var scope = app.Services.CreateScope())
{
    DatabaseInitializer.Initialize(scope.ServiceProvider.GetRequiredService<MoviesDbContext>());
}

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "movie",
    pattern: "{controller}/{movieurl}",
    defaults: new { controller = "Movies", action = "Movie" });

app.MapHub<TicketHub>("/ticketHub");

await app.RunAsync();