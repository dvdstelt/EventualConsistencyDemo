using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class MoviesController(MoviesDbContext db) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        public async Task<IActionResult> Movie(string movieurl)
        {
            var movie = await db.Movies.SingleOrDefaultAsync(s => s.UrlTitle == movieurl);

            if (movie == null)
                return NotFound();

            var vm = new MovieViewModel
            {
                Movie = movie,
                Theaters = TheatersContext.GetTheaters(),
                Reviews = await db.Reviews.Where(r => r.MovieIdentifier == movie.Id).ToListAsync()
            };

            return View(vm);
        }
    }
}
