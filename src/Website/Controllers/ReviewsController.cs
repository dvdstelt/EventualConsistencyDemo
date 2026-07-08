using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace EventualConsistencyDemo.Controllers
{
    public class ReviewsController(MoviesDbContext db) : Controller
    {
        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        // GET: Reviews/gameofthrones
        public async Task<IActionResult> Movie(string movieurl)
        {
            var movie = await db.Movies.SingleOrDefaultAsync(s => s.UrlTitle == movieurl);

            if (movie == null)
                return NotFound();

            var vm = new ReviewViewModel
            {
                Movie = movie,
                Reviews = await db.Reviews.Where(s => s.MovieIdentifier == movie.Id).ToListAsync()
            };

            return View(vm);
        }
    }
}
