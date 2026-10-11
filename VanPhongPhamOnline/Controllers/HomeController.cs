using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VanPhongPhamOnline.Models;

namespace VanPhongPhamOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/404")]
        [Route("Home/PageNotFound")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
