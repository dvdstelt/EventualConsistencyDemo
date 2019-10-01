using System.Linq;
using System.Text;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public string Movie(IFormCollection collection)
        {
            var sb = new StringBuilder();
            foreach (var item in collection)
            {
                sb.Append(item + "|");
            }

            //  return JsonConvert.SerializeObject(chk);

            return sb.ToString();
        }
    }
}