using EventShopApp.Areas.RegisteredUser.ViewModels;
using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EventShopApp.Areas.RegisteredUser.Controllers
{
    [Area("RegisteredUser")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder = "desc")
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var client = _context.Clients.FirstOrDefault(c => c.Email == user.Email);

            if (client == null)
            {
                TempData["Message"] = "You have to be a client in order to visit the 'My Profile' page.";
                return RedirectToAction("Index", "Home", new {area = ""});
            }

            var rawOrders = _context.Orders
                .Where(o => o.ClientId == client.Id)
                .Join(_context.OrderDetails,
                     order => order.Id,
                     details => details.OrderId,
                    (order, details) => new
                    {
                        Order = order,
                         Details = details
                    })
                .ToList(); 

            
            var orderDetails = rawOrders.Select(orderDetail =>
            {
                string itemName;
                if (orderDetail.Details.Type == Enums.OrderType.Flower)
                {
                    var flower = _context.Flowers.FirstOrDefault(f => f.Id == orderDetail.Details.FlowerId);
                    itemName = flower != null ? flower.FlowerType : "Unknown";
                }
                else
                {
                    var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == orderDetail.Details.ArrangementItemsId);
                    itemName = arrangement != null ? arrangement.ArrangementItemType : "Unknown";
                }

                return new OrderDetailsViewModel
                {
                    ItemType = orderDetail.Details.Type.ToString(),
                    ItemName = itemName,
                    Quantity = orderDetail.Details.OrderedFlowerQuantity ?? orderDetail.Details.OrderedArrangementQuantity ?? 0,
                    DateOfOrder = orderDetail.Order.DateOfOrder,
                    DeadLineDate = orderDetail.Order.DeadLineDate,
                    Status = orderDetail.Order.Status.ToString()
                };
            }).ToList();

            
            orderDetails = sortOrder.ToLower() switch
            {
                "asc" => orderDetails.OrderBy(o => o.DateOfOrder).ToList(),
                _ => orderDetails.OrderByDescending(o => o.DateOfOrder).ToList()
            };

            var model = new ProfileViewModel
            {
                Email = client.Email,
                Name = client.Name,
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                Orders = orderDetails
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSurname([FromBody] UpdateFieldViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var client = _context.Clients.FirstOrDefault(c => c.Email == user.Email);
            if (client == null) return NotFound();

            client.Surname = model.Value;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody] UpdateFieldViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var client = _context.Clients.FirstOrDefault(c => c.Email == user.Email);
            if (client == null) return NotFound();

            client.PhoneNumber = model.Value;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateFieldViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var client = _context.Clients.FirstOrDefault(c => c.Email == user.Email);
            if (client == null) return NotFound();

            client.Address = model.Value;
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
