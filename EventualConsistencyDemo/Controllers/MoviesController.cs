using System.Linq;
using System.Text;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class MoviesController : Controller
    {
        //private readonly TheatersContext theatersContext;
        //private readonly MoviesContext moviesContext;

        //public MoviesController(MoviesContext moviesContext, TheatersContext theatersContext)
        //{
        //    this.theatersContext = theatersContext;
        //    this.moviesContext = moviesContext;
        //}

        public ActionResult Index()
        {
            return View(MoviesContext.GetMovies());
        }

        public ActionResult Movie(string movieurl)
        {
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