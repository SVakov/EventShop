using EventShopApp.Models;
using EventShopApp.Services;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventShopApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFlowerService _flowerService;
        private readonly IArrangementService _arrangementService;

        public HomeController(ILogger<HomeController> logger, IFlowerService flowerService, IArrangementService arrangementService)
        {
            _logger = logger;
            _flowerService = flowerService;
            _arrangementService = arrangementService;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch flower and arrangement images
            var flowers = await _flowerService.GetAllFlowerImagesAsync();
            var arrangements = await _arrangementService.GetAllArrangementImagesAsync();

            var model = new HomeViewModel
            {
                Flowers = flowers,
                Arrangements = arrangements
            };

            return View(model);
        }

        public IActionResult Privacy()
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
