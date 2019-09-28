using System.Linq;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventualConsistencyDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Movies movies;

        public MoviesController(Movies movies)
        {
            this.movies = movies;
        }

        public ActionResult Index()
        {
            return View(movies.GetMovies());
        }

        public ActionResult Movie(string movieurl)
        {
            return View(movies.GetMovies().Single(s => s.UrlTitle == movieurl));
        }
    }
}