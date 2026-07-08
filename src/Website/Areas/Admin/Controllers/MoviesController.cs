using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace EventualConsistencyDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController(MoviesDbContext db, IWebHostEnvironment environment) : Controller
    {
        static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

        public async Task<IActionResult> Index()
        {
            var movies = await db.Movies.OrderBy(m => m.Title).ToListAsync();
            return View(movies);
        }

        public IActionResult Create() => View(new MovieEditModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieEditModel model)
        {
            if (string.IsNullOrEmpty(model.PosterData))
                ModelState.AddModelError(nameof(model.PosterData), "A poster image is required.");
            if (model.BackgroundFile == null)
                ModelState.AddModelError(nameof(model.BackgroundFile), "A background image is required.");
            if (await db.Movies.AnyAsync(m => m.UrlTitle == model.UrlTitle))
                ModelState.AddModelError(nameof(model.UrlTitle), "A movie with this URL title already exists.");
            ValidateImage(model.BackgroundFile, nameof(model.BackgroundFile));

            if (!ModelState.IsValid)
                return View(model);

            var movie = new Movie { Id = Guid.NewGuid() };
            Apply(model, movie);
            movie.Image = await SavePosterAsync(model.PosterData!, model.UrlTitle);
            movie.Background = await SaveImageAsync(model.BackgroundFile!, "background", model.UrlTitle);

            db.Movies.Add(movie);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await db.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(ToModel(movie));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieEditModel model)
        {
            var movie = await db.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            if (await db.Movies.AnyAsync(m => m.UrlTitle == model.UrlTitle && m.Id != id))
                ModelState.AddModelError(nameof(model.UrlTitle), "A movie with this URL title already exists.");
            ValidateImage(model.BackgroundFile, nameof(model.BackgroundFile));

            if (!ModelState.IsValid)
            {
                model.ExistingImage = movie.Image;
                model.ExistingBackground = movie.Background;
                return View(model);
            }

            Apply(model, movie);
            if (!string.IsNullOrEmpty(model.PosterData))
                movie.Image = await SavePosterAsync(model.PosterData, model.UrlTitle);
            if (model.BackgroundFile != null)
                movie.Background = await SaveImageAsync(model.BackgroundFile, "background", model.UrlTitle);

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await db.Movies.FindAsync(id);
            if (movie != null)
            {
                db.Reviews.RemoveRange(db.Reviews.Where(r => r.MovieIdentifier == id));
                db.Movies.Remove(movie);
                await db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        void ValidateImage(IFormFile? file, string key)
        {
            if (file == null)
                return;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                ModelState.AddModelError(key, $"Only image files are allowed ({string.Join(", ", AllowedExtensions)}).");
        }

        async Task<string> SaveImageAsync(IFormFile file, string folder, string urlTitle)
        {
            var fileName = urlTitle + Path.GetExtension(file.FileName).ToLowerInvariant();
            var directory = Path.Combine(environment.WebRootPath, "img", folder);
            Directory.CreateDirectory(directory);

            await using var stream = System.IO.File.Create(Path.Combine(directory, fileName));
            await file.CopyToAsync(stream);
            return fileName;
        }

        // Saves the cropper's data URL ("data:image/jpeg;base64,....") as {urlTitle}.jpg.
        async Task<string> SavePosterAsync(string dataUrl, string urlTitle)
        {
            var commaIndex = dataUrl.IndexOf(',');
            var base64 = commaIndex >= 0 ? dataUrl[(commaIndex + 1)..] : dataUrl;
            var bytes = Convert.FromBase64String(base64);

            var fileName = urlTitle + ".jpg";
            var directory = Path.Combine(environment.WebRootPath, "img", "posters");
            Directory.CreateDirectory(directory);
            await System.IO.File.WriteAllBytesAsync(Path.Combine(directory, fileName), bytes);
            return fileName;
        }

        static void Apply(MovieEditModel model, Movie movie)
        {
            movie.Title = model.Title;
            movie.UrlTitle = model.UrlTitle;
            movie.Description = model.Description;
            movie.Rating = model.Rating;
            movie.MovieDetails = model.MovieDetails;
            movie.ReleaseDate = model.ReleaseDate;
            movie.PricePerTicket = model.PricePerTicket;
            movie.PopularityScore = model.PopularityScore;
            movie.TicketType = model.TicketType;
            movie.Icons = SplitCsv(model.Icons);
            movie.Showtimes = SplitCsv(model.Showtimes);
        }

        static MovieEditModel ToModel(Movie movie) => new()
        {
            Id = movie.Id,
            Title = movie.Title,
            UrlTitle = movie.UrlTitle,
            Description = movie.Description,
            Rating = movie.Rating,
            MovieDetails = movie.MovieDetails,
            ReleaseDate = movie.ReleaseDate,
            PricePerTicket = movie.PricePerTicket,
            PopularityScore = movie.PopularityScore,
            TicketType = movie.TicketType,
            Icons = string.Join(", ", movie.Icons),
            Showtimes = string.Join(", ", movie.Showtimes),
            ExistingImage = movie.Image,
            ExistingBackground = movie.Background
        };

        static List<string> SplitCsv(string value) =>
            value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
