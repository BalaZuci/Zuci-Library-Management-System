using LibraryMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryMVC.Controllers
{
    /// <summary>
    /// this controller controls the home and privacy pages.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// For showing the home page
        /// </summary>
        /// <returns>view of home page</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// for showing the privacy page
        /// </summary>
        /// <returns>view of the privacy page</returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// for showing the error page
        /// </summary>
        /// <returns>view of error page</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
