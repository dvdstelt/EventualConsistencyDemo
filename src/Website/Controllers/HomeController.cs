using System.Diagnostics;
using System.Threading.Tasks;
using EventualConsistencyDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;

namespace EventualConsistencyDemo.Controllers
{
    public class HomeController(MoviesDbContext db) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await db.Movies.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
