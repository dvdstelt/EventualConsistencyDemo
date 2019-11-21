using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly LiteRepository db;

        public ReviewsController(LiteRepository db)
        {
            this.db = db;
        }

        // GET: Reviews
        public ActionResult Index()
        {
            return View(db.Fetch<Movie>());
        }

        // GET: Reviews/gameofthrones
        public ActionResult Movie(string movieurl)
        {
            var vm = new ReviewViewModel();

            vm.Movie = db.Fetch<Movie>().Single(s => s.UrlTitle == movieurl);
            vm.Reviews = db.Fetch<Review>().Where(s => s.MovieIdentifier == vm.Movie.Id);

            return View(vm);
        }
    }
}