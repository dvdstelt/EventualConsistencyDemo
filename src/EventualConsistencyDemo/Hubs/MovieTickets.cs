using System;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Shared.Entities;

namespace EventualConsistencyDemo.Hubs
{
    public class MovieTickets
    {
        readonly IHubContext<TicketHub> hub;
        readonly LiteRepository db;
        readonly IMemoryCache memoryCache;

        public MovieTickets(IHubContext<TicketHub> hub, LiteRepository db, IMemoryCache memoryCache)
        {
            this.hub = hub;
            this.db = db;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// After ticket registration, immediately verify if it's a lottery ticket. 
        /// </summary>
        public async Task ReportOnLottery(MovieTicket ticket, string connectionId)
        {
            var movieId = Guid.Parse(ticket.MovieId);

            var movie = db.Query<Movie>()
                .Where(s => s.Id == movieId)
                .SingleOrDefault();

            if (movie == null)
                throw new ArgumentException($"Movie [{movieId}] can't be found", nameof(movieId));
            
            if (movie.TicketType == TicketType.DrawingTicket)
            {
                await hub.Clients.Client(connectionId).SendAsync("OrderSubmission", "Thanks for the registration!");
            }
        }
    }
}