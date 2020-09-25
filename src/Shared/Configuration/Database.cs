using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LiteDB;
using Shared.Entities;

namespace Shared.Configuration
{
    public static class Database
    {
        public static string DatabaseConnectionstring
        {
            get
            {
                var storagePath = FindStoragePath();
                Directory.CreateDirectory(storagePath);
                var databaseLocation = Path.Combine(storagePath, DatabaseName);
                databaseLocation = $"Filename={databaseLocation}; Connection=shared";

                return databaseLocation;
            }
        }

        public static void Setup()
        {
            using var db = new LiteDatabase(DatabaseConnectionstring);
            var movieCollection = db.GetCollection<Movie>("movie");
            var reviewCollection = db.GetCollection<Review>("review");
            if (movieCollection.Count() == 0)
            {
                var movies = DefaultData.GetDefaultMovies();
                movieCollection.Insert(movies);
                reviewCollection.Insert(DefaultData.GetDefaultReviews(movies));

                movieCollection.EnsureIndex(x => x.Id);
                movieCollection.EnsureIndex(x => x.UrlTitle);

                reviewCollection.EnsureIndex(x => x.Id);
                reviewCollection.EnsureIndex(x => x.MovieIdentifier);
            }
        }

        static string FindStoragePath()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            while (true)
            {
                // Finding a solution file takes precedence
                if (Directory.EnumerateFiles(directory).Any(file => file.EndsWith(".sln")))
                {
                    return Path.Combine(directory, DefaultDatabaseDirectory);
                }

                // When no solution file was found try to find a database directory
                var databaseDirectory = Path.Combine(directory, DefaultDatabaseDirectory);
                if (Directory.Exists(databaseDirectory))
                {
                    return databaseDirectory;
                }

                var parent = Directory.GetParent(directory);

                if (parent == null)
                {
                    // throw for now. if we discover there is an edge then we can fix it in a patch.
                    throw new Exception($"Unable to determine the storage directory path for the database due to the absence of a solution file. Please create a '{DefaultDatabaseDirectory}' directory in one of this project’s parent directories.");
                }

                directory = parent.FullName;
            }
        }

        const string DefaultDatabaseDirectory = ".database";
        const string DatabaseName = "eventual-consistency.db";
    }
}
