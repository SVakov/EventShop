using EventShopApp.Data;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ArrangementsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ArrangementsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var arrangements = await _context.ArrangementItems
            .Where(a => a.ArrangementItemsQuantity > 0)
            .Select(a => new ArrangementViewModel
            {
                Id = a.Id,
                ArrangementItemType = a.ArrangementItemType,
                Price = a.Price,
                Description = a.Description.Length > 20 ? a.Description.Substring(0, 20) + "..." : a.Description,
                ArrangementItemImageUrl = a.ArrangementItemImageUrl,
                ArrangementItemsQuantity = a.ArrangementItemsQuantity
            })
            .ToListAsync();

        return View(arrangements);
    }

    public async Task<IActionResult> Details(int id)
    {
        var arrangement = await _context.ArrangementItems
            .Where(a => a.Id == id)
            .Select(a => new ArrangementViewModel
            {
                Id = a.Id,
                ArrangementItemType = a.ArrangementItemType,
                Price = a.Price,
                Description = a.Description,
                ArrangementItemImageUrl = a.ArrangementItemImageUrl,
                ArrangementItemsQuantity = a.ArrangementItemsQuantity
            })
            .FirstOrDefaultAsync();

        if (arrangement == null)
        {
            return NotFound();
        }

        return View(arrangement);
    }
}