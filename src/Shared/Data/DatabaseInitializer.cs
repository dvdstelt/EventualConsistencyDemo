using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Shared.Data
{
    public static class DatabaseInitializer
    {
        // Creates the schema from the entity model and seeds the movies/reviews from Seed.sql.
        // Idempotent: EnsureCreated is a no-op once the tables exist, and the seed script uses
        // INSERT OR IGNORE, so it is safe to call from both endpoints on startup.
        public static void Initialize(MoviesDbContext db)
        {
            db.Database.EnsureCreated();

            if (!db.Movies.Any())
            {
                db.Database.ExecuteSqlRaw(ReadSeedScript());
            }
        }

        static string ReadSeedScript()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(name => name.EndsWith("Seed.sql"));
            using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
