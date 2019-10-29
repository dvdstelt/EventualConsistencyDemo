using System.Linq;
using System.Text;
using EventualConsistencyDemo.Models;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly LiteRepository db;

        public MoviesController(LiteRepository db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View(MoviesContext.GetMovies());
        }

        public ActionResult Movie(string movieurl)
        {
            var movie = db.Query<Movie>()
                            .Where(s => s.UrlTitle == movieurl)
                            .SingleOrDefault();

            var vm = new MovieViewModel();
            vm.Movie = MoviesContext.GetMovies().Single(s => s.UrlTitle == movieurl);
            vm.Theaters = TheatersContext.GetTheaters();

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