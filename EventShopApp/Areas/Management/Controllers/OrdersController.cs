using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventShopApp.Enums;

namespace EventShopApp.Areas.Management.Controllers
{
    [Area("Management")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
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

        
        public async Task<IActionResult> Add()
        {
            
            ViewBag.Flowers = await _context.Flowers.ToListAsync();
            ViewBag.Arrangements = await _context.ArrangementItems.ToListAsync();

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FindAsync(orderViewModel.ClientId);
                if (client == null)
                {
                    client = new Client
                    {
                        Name = orderViewModel.ClientName,
                        Email = orderViewModel.ClientEmail,
                        PhoneNumber = orderViewModel.ClientPhoneNumber,
                        Address = orderViewModel.ClientAddress
                    };

                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync(); 
                }

                var order = new Order
                {
                    ClientId = client.Id,
                    DateOfOrder = DateTime.Now,
                    DeadLineDate = orderViewModel.DeadLineDate,
                    Status = OrderStatus.Pending 
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); 

                foreach (var detail in orderViewModel.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        FlowerId = detail.FlowerId,
                        OrderedFlowerQuantity = detail.OrderedFlowerQuantity,
                        ArrangementItemsId = detail.ArrangementItemsId,
                        OrderedArrangementQuantity = detail.OrderedArrangementQuantity,
                        Type = detail.Type,
                        IsPrepayed = orderViewModel.IsPrepayed 
                    };

                    _context.OrderDetails.Add(orderDetail);

                    if (detail.FlowerId.HasValue)
                    {
                        var flower = await _context.Flowers
                            .FirstOrDefaultAsync(f => f.Id == detail.FlowerId.Value);

                        if (flower != null && flower.FlowerQuantity >= detail.OrderedFlowerQuantity)
                        {
                            flower.FlowerQuantity -= detail.OrderedFlowerQuantity.Value; 
                            _context.Flowers.Update(flower);
                        }
                        else
                        {
                            ModelState.AddModelError("FlowerInventory", "Not enough stock for the selected flower.");
                            return View(orderViewModel); 
                        }
                    }

                    if (detail.ArrangementItemsId.HasValue)
                    {
                        var arrangementItem = await _context.ArrangementItems
                            .FirstOrDefaultAsync(a => a.Id == detail.ArrangementItemsId.Value);

                        if (arrangementItem != null && arrangementItem.ArrangementItemsQuantity >= detail.OrderedArrangementQuantity)
                        {
                            arrangementItem.ArrangementItemsQuantity -= detail.OrderedArrangementQuantity.Value; 
                            _context.ArrangementItems.Update(arrangementItem);
                        }
                        else
                        {
                            ModelState.AddModelError("ArrangementInventory", "Not enough stock for the selected arrangement.");
                            return View(orderViewModel); 
                        }
                    }
                }

                await _context.SaveChangesAsync(); 

                return RedirectToAction(nameof(Index)); 
            }

            ViewBag.Flowers = await _context.Flowers.ToListAsync();
            ViewBag.Arrangements = await _context.ArrangementItems.ToListAsync();

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            
            order.Status = Enum.Parse<OrderStatus>(status);

            _context.Update(order);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }



    }
}
