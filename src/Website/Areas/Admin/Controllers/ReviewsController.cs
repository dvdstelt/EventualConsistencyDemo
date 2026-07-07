using System;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace EventualConsistencyDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewsController(MoviesDbContext db, TimeProvider timeProvider) : Controller
    {
        public async Task<IActionResult> Index(Guid movieId)
        {
            var movie = await db.Movies.FindAsync(movieId);
            if (movie == null)
                return NotFound();

            var model = new AdminReviewsModel
            {
                MovieId = movie.Id,
                MovieTitle = movie.Title,
                Reviews = await db.Reviews
                    .Where(r => r.MovieIdentifier == movieId)
                    .OrderByDescending(r => r.ReviewedAt)
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid movieId, string description, string? postedByEmail)
        {
            if (!await db.Movies.AnyAsync(m => m.Id == movieId))
                return NotFound();

            if (!string.IsNullOrWhiteSpace(description))
            {
                db.Reviews.Add(new Review
                {
                    Id = Guid.NewGuid(),
                    MovieIdentifier = movieId,
                    Description = description.Trim(),
                    PostedByEmail = string.IsNullOrWhiteSpace(postedByEmail) ? null : postedByEmail.Trim(),
                    ReviewedAt = timeProvider.GetLocalNow().DateTime
                });
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { movieId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid movieId)
        {
            var review = await db.Reviews.FindAsync(id);
            if (review != null)
            {
                db.Reviews.Remove(review);
                await db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { movieId });
        }
    }
}
