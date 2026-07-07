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
            return View(db.Query<Movie>().ToList());
        }

        // GET: Reviews/gameofthrones
        public ActionResult Movie(string movieurl)
        {
            var movie = db.Query<Movie>().Where(s => s.UrlTitle == movieurl).SingleOrDefault();

            if (movie == null)
                return NotFound();

            var vm = new ReviewViewModel
            {
                Movie = movie,
                Reviews = db.Query<Review>().Where(s => s.MovieIdentifier == movie.Id).ToEnumerable()
            };

            return View(vm);
        }
    }
}