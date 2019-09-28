using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventualConsistencyDemo.Models;

namespace EventualConsistencyDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly Movies movies;

        public HomeController(Movies movies)
        {
            this.movies = movies;
        }

        public IActionResult Index()
        {
            return View(movies.GetMovies());
        }

        public IActionResult Movies()
        {
            return View();
        }

        public IActionResult Theaters()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
