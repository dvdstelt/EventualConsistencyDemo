using EventualConsistencyDemo.Hubs;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Extensions.DependencyInjection;
using Shared.Configuration;

namespace EventualConsistencyDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSignalR();
            //services.AddMemoryCache();
            //services.AddSingleton<MoviesContext>();
            //services.AddSingleton<TheatersContext>();

            var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
            endpointConfiguration.ApplyCommonConfiguration(routingConfig => 
            {
                routingConfig.RouteToEndpoint(typeof(Shared.Commands.SubmitOrder), "server");
            });

            services.AddNServiceBus(endpointConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TicketHub>("/ticketHub");

                endpoints.MapControllerRoute(
                    name: "movie",
                    pattern: "movies/{movieurl?}", 
                    defaults: new { controller = "Movies", action = "Movie" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");


            });
        }
    }
}
