using EventShopApp.Services;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class ArrangementsController : Controller
    {
        private readonly IArrangementService _arrangementService;

        public ArrangementsController(IArrangementService arrangementService)
        {
            _arrangementService = arrangementService;
        }

        // Use the service to fetch all available arrangements
        public async Task<IActionResult> Index()
        {
            var arrangements = await _arrangementService.GetAllAvailableArrangementsAsync();
            return View(arrangements);
        }

        // Use the service to fetch arrangement details
        public async Task<IActionResult> Details(int id)
        {
            var arrangement = await _arrangementService.GetArrangementDetailsByIdAsync(id);

            if (arrangement == null)
            {
                return NotFound();
            }

            return View(arrangement);
        }
    }
}
