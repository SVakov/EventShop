using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Controllers
{
    public class FlowersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlowersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action to display the list of flowers
        public async Task<IActionResult> Index()
        {
            var flowers = await _context.Flowers
                .Where(f => f.FlowerQuantity > 0)
                .Select(f => new FlowerViewModel
                {
                     Id = f.Id,
                    FlowerType = f.FlowerType,
                    Price = f.Price,
                    Description = f.Description.Length > 20 ? f.Description.Substring(0, 20) + "..." : f.Description,
                    FlowerImageUrl = f.FlowerImageUrl
                })
        .ToListAsync();

            return View(flowers);
        }

        // Action to display detailed view of a specific flower
        public async Task<IActionResult> Details(int id)
        {
            var flower = await _context.Flowers
        .Where(f => f.Id == id)
        .Select(f => new FlowerViewModel
        {
            Id = f.Id,
            FlowerType = f.FlowerType,
            Price = f.Price,
            Description = f.Description,
            FlowerImageUrl = f.FlowerImageUrl,
            FlowerQuantity = f.FlowerQuantity
        })
        .FirstOrDefaultAsync();

            if (flower == null)
            {
                return NotFound();
            }

            return View(flower);
        }
    }
}
