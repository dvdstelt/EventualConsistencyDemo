using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Hubs;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;
using static Shared.Configuration.Configuration;

namespace EventualConsistencyDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IEndpointInstance endpointInstance;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSignalR();
            services.AddMemoryCache();
            services.AddSingleton<Movies>();
            services.AddSingleton<Theaters>();
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
                    pattern: "movies/{movieurl}", 
                    defaults: new { controller = "Movies", action = "Movie"});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");


            });
        }

        private void StartEndpoint()
        {
            var endpointConfiguration = ConfigureEndpoint("EventualConsistencyDemo");

            endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
        }
    }
}
