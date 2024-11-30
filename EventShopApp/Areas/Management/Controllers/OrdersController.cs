using EventShopApp.Models;
using EventShopApp.Enums;
using EventShopApp.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventShopApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventShopApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index page - List of orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Flower)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ArrangementItem)
                .ToListAsync();

            return View(orders);
        }

        // Add new order form
        public IActionResult Add()
        {
            return View();
        }

        // Save new order and order details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the client already exists
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == model.ClientEmail);

                if (client == null)
                {
                    // Create new client if not found
                    client = new Client
                    {
                        Name = model.ClientName,
                        Email = model.ClientEmail,
                        PhoneNumber = model.ClientPhoneNumber
                    };
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                }

                // Create new order
                var order = new Order
                {
                    ClientId = client.Id,
                    DateOfOrder = DateTime.Now,
                    DeadLineDate = model.DeadLineDate,
                    Status = OrderStatus.Pending
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add OrderDetails
                if (model.OrderDetails != null)
                {
                    foreach (var detail in model.OrderDetails)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            FlowerId = detail.FlowerId,
                            OrderedFlowerQuantity = detail.OrderedFlowerQuantity,
                            ArrangementItemsId = detail.ArrangementItemsId,
                            OrderedArrangementQuantity = detail.OrderedArrangementQuantity,
                            Type = detail.Type,
                            IsPrepayed = model.IsPrepayed
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // Edit order status
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            var viewModel = new OrderStatusViewModel
            {
                OrderId = order.Id,
                Status = order.Status
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _context.Orders.FindAsync(model.OrderId);
                if (order == null) return NotFound();

                order.Status = model.Status;
                _context.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
