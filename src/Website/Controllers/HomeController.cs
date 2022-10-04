using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventualConsistencyDemo.Models;
using LiteDB;
using Shared.Entities;

namespace EventualConsistencyDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteRepository db;

        public HomeController(LiteRepository db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(db.Query<Movie>().ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            throw new System.NotImplementedException();
        }
    }
}
