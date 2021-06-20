using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WalmartIntergration.Models;
using WalmartIntergration.Services;

namespace WalmartIntergration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WalmartItemService _walmartItemService;

        public HomeController(ILogger<HomeController> logger, WalmartItemService walmartItemService)
        {
            _logger = logger;
            _walmartItemService = walmartItemService;
        }

        public IActionResult Index()
        {
            return View(_walmartItemService.FetchItems().Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
