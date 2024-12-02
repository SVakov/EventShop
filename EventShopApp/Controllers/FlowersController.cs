using EventShopApp.Services;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class FlowersController : Controller
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        // Fetch the list of flowers for the Index page
        public async Task<IActionResult> Index()
        {
            var flowers = await _flowerService.GetAllAvailableFlowersAsync(); 
            return View(flowers); 
        }

        // Fetch details for a specific flower
        public async Task<IActionResult> Details(int id)
        {
            var flower = await _flowerService.GetFlowerDetailsByIdAsync(id);

            if (flower == null)
            {
                return NotFound();
            }

            return View(flower); 
        }
    }
}
