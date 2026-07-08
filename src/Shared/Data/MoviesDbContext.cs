using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Entities;

namespace Shared.Data
{
    public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Store Guids as canonical lower-case text so the SQL seed script is deterministic
            // and its ids compare equal to the ones EF generates at runtime.
            configurationBuilder.Properties<Guid>().HaveConversion<GuidToStringConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store the enum by name ('RegularTicket'/'DrawingTicket') so the seed reads clearly.
            modelBuilder.Entity<Movie>().Property(m => m.TicketType).HasConversion<string>();

            modelBuilder.Entity<Review>().HasIndex(r => r.MovieIdentifier);
        }
    }
}
