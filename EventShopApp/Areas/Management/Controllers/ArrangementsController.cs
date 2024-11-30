using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services;
using EventShopApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize]
    public class ArrangementsController : Controller
    {
        private readonly IArrangementService _arrangementService;

        public ArrangementsController(IArrangementService arrangementService)
        {
            _arrangementService = arrangementService;
        }

        public async Task<IActionResult> Index(string filter = "all", string sortOrder = "price-asc")
        {
            var arrangements = await _arrangementService.GetAllArrangements();

            // Apply filters
            if (filter == "available")
                arrangements = arrangements.Where(a => a.IsAvailable).ToList();
            else if (filter == "unavailable")
                arrangements = arrangements.Where(a => !a.IsAvailable).ToList();

            // Apply sorting
            arrangements = sortOrder switch
            {
                "price-asc" => arrangements.OrderBy(a => a.Price).ToList(),
                "price-desc" => arrangements.OrderByDescending(a => a.Price).ToList(),
                "quantity-asc" => arrangements.OrderBy(a => a.ArrangementItemsQuantity).ToList(),
                "quantity-desc" => arrangements.OrderByDescending(a => a.ArrangementItemsQuantity).ToList(),
                _ => arrangements.OrderBy(a => a.ArrangementItemType).ToList()
            };

            ViewBag.Filter = filter;
            ViewBag.SortOrder = sortOrder;

            return View(arrangements);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ArrangementItem model)
        {
            if (ModelState.IsValid)
            {
                await _arrangementService.AddArrangement(model);
                return Ok(new { success = true, message = "Arrangement added successfully" });
            }
            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var arrangement = await _arrangementService.GetArrangementById(id);
            if (arrangement == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = arrangement.Id,
                price = arrangement.Price,
                arrangementItemsQuantity = arrangement.ArrangementItemsQuantity,
                description = arrangement.Description,
                arrangementItemImageUrl = arrangement.ArrangementItemImageUrl
            });
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ArrangementItem model)
        {
            if (ModelState.IsValid)
            {
                var arrangement = await _arrangementService.GetArrangementById(model.Id);
                if (arrangement == null)
                {
                    return NotFound(new { success = false, message = "Arrangement not found." });
                }

                arrangement.Price = model.Price; // Ensure Price is updated
                arrangement.ArrangementItemsQuantity = model.ArrangementItemsQuantity;
                arrangement.Description = model.Description;
                arrangement.ArrangementItemImageUrl = model.ArrangementItemImageUrl;

                await _arrangementService.UpdateArrangement(arrangement);

                return Ok(new { success = true, message = "Arrangement updated successfully" });
            }

            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _arrangementService.SoftDeleteArrangement(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BringBack(int id)
        {
            await _arrangementService.BringBackArrangement(id);
            return Ok(new { success = true, message = "Arrangement restored successfully" });
        }
    }
}
