using System.Linq;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventualConsistencyDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Theaters theaters;
        private readonly Movies movies;

        public MoviesController(Movies movies, Theaters theaters)
        {
            this.theaters = theaters;
            this.movies = movies;
        }

        public ActionResult Index()
        {
            return View(movies.GetMovies());
        }

        public ActionResult Movie(string movieurl)
        {
            var vm = new MovieViewModel();
            vm.Movie = movies.GetMovies().Single(s => s.UrlTitle == movieurl);
            vm.Theaters = theaters.GetTheaters();

            return View(vm);
        }
    }
}