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
    public class FlowersController : Controller
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        // GET: Management/Flowers
        public async Task<IActionResult> Index(string filter = "all", string sortOrder = "price-asc")
        {
            var userEmail = User.Identity?.Name;

            var userRole = await _flowerService.GetEmployeeRoleByEmail(userEmail);
            if (userRole == null)
            {
                return Forbid(); 
            }

            

            var flowers = await _flowerService.GetAllFlowers();

            if (filter == "available")
            {
                flowers = flowers.Where(f => f.IsAvailable).ToList();
            }
            else if (filter == "unavailable")
            {
                flowers = flowers.Where(f => !f.IsAvailable).ToList();
            }

            flowers = sortOrder switch
            {
                "price-asc" => flowers.OrderBy(f => f.Price).ToList(),
                "price-desc" => flowers.OrderByDescending(f => f.Price).ToList(),
                "quantity-asc" => flowers.OrderBy(f => f.FlowerQuantity).ToList(),
                "quantity-desc" => flowers.OrderByDescending(f => f.FlowerQuantity).ToList(),
                _ => flowers.OrderBy(f => f.FlowerType).ToList()
            };

            ViewBag.Filter = filter;
            ViewBag.SortOrder = sortOrder;

            return View(flowers);
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Flower model)
        {
            if (ModelState.IsValid)
            {
                await _flowerService.AddFlower(model);
                return Ok(new { success = true, message = "Flower added successfully" });
            }
            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        // GET: Management/Flowers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var flower = await _flowerService.GetFlowerById(id);
            if (flower == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = flower.Id,
                flowerQuantity = flower.FlowerQuantity,
                description = flower.Description,
                flowerImageUrl = flower.FlowerImageUrl
            });
        }

        // POST: Management/Flowers/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Flower model)
        {
            if (ModelState.IsValid)
            {
                var flower = await _flowerService.GetFlowerById(model.Id);
                if (flower == null)
                {
                    return NotFound(new { success = false, message = "Flower not found." });
                }

                flower.FlowerQuantity = model.FlowerQuantity;
                flower.Description = model.Description;
                flower.FlowerImageUrl = model.FlowerImageUrl;

                await _flowerService.UpdateFlower(flower);

                return Ok(new { success = true, message = "Flower updated successfully" });
            }

            return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }


        // POST: Management/Flowers/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _flowerService.SoftDeleteFlower(id);
            return RedirectToAction("Index");
        }

        // POST: Management/Flowers/BringBack/5
        [HttpPost]
        public async Task<IActionResult> BringBack(int id)
        {
            await _flowerService.BringBackFlower(id);
            return RedirectToAction("Index");
        }
    }
}
