using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;

namespace Shared.Data
{
    public static class MoviesDatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddMoviesDatabase(this IServiceCollection services)
        {
            return services.AddDbContext<MoviesDbContext>(options => options
                .UseSqlite(Database.ConnectionString)
                .UseSeeding((context, _) => SeedData.Seed(context))
                .UseAsyncSeeding((context, _, cancellationToken) => SeedData.SeedAsync(context, cancellationToken)));
        }
    }
}
