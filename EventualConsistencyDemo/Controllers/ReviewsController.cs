using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class ReviewsController : Controller
    {
        // GET: Reviews
        public ActionResult Index()
        {
            return View(MoviesContext.GetMovies());
        }

        // GET: Reviews/gameofthrones
        public ActionResult Movie(string movieurl)
        {
            var vm = new ReviewViewModel();
            vm.Movie = MoviesContext.GetMovies().Single(s => s.UrlTitle == movieurl);
            vm.Reviews = ReviewsContext.GetReviews().Where(s => s.MovieIdentifier == vm.Movie.Id);

            return View(vm);
        }
    }
}