using EventualConsistencyDemo.Hubs;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Shared.Configuration;

var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
endpointConfiguration.ApplyCommonConfiguration(routingConfig =>
{
    routingConfig.RouteToEndpoint(typeof(Shared.Commands.SubmitOrder), "server");
});

var builder = WebApplication.CreateBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var services = builder.Services;
services.AddControllersWithViews();

services.AddSignalR(o => o.EnableDetailedErrors = true);

services.AddScoped(_ => new LiteRepository(Database.DatabaseConnectionstring));
services.AddScoped<MovieTickets>();

services.AddMemoryCache();

// Create the LiteDb database so we can work with some default movies.
Database.Setup();

var app = builder.Build();

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