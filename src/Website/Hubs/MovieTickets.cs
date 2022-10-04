using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using LiteDB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Entities;

namespace EventualConsistencyDemo.Hubs
{
    public class MovieTickets
    {
        readonly IHubContext<TicketHub> hub;
        readonly LiteRepository db;
        readonly ILogger<MovieTickets> logger;

        public MovieTickets(IHubContext<TicketHub> hub, LiteRepository db, ILogger<MovieTickets> logger)
        {
            this.hub = hub;
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// After ticket registration, immediately verify if it's a lottery ticket. 
        /// </summary>
        public async Task ReportOnLottery(MovieTicket ticket, string connectionId)
        {
            var movieId = Guid.Parse(ticket.MovieId);
            var theaterId = Guid.Parse(ticket.TheaterId);

            var movie = db.Query<Movie>()
                .Where(s => s.Id == movieId)
                .Single();

            if (movie.TicketType == TicketType.DrawingTicket)
            {
                var theater = TheatersContext.GetTheaters().Single(s => s.Id == theaterId);                
                
                var message = new
                {
                    Theater = theater.Name,
                    MovieTitle = movie.Title,
                    Time = ticket.Time,
                    NumberOfTickets = ticket.NumberOfTickets,
                    DrawingDate = DateTime.Now.AddDays(14).ToString("dddd, dd MMMM", CultureInfo.InvariantCulture)
                };                
 
                logger.LogInformation("Sending OrderedLotteryTicket", message);                
                
                await hub.Clients.Client(connectionId).SendAsync("OrderedLotteryTicket", message);
            }
        }
    }
}