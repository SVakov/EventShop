using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItemViewModel> CartItems = new List<CartItemViewModel>();
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display the cart
        public IActionResult Index()
        {
            return View(CartItems);
        }

        // Add item to cart
        [HttpPost]
        public IActionResult AddToCart(CartItemViewModel item)
        {
            var existingItem = CartItems.Find(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                CartItems.Add(item);
            }
            return Json(new { success = true });
        }

        public IActionResult Order()
        {
            return View(new OrderViewModel());
        }

        // Submits the order and saves it to the database
        [HttpPost]
        public async Task<IActionResult> SubmitOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the client already exists by email
                var client = _context.Clients.FirstOrDefault(c => c.Email == model.Email);

                // If client doesn't exist, create a new client
                if (client == null)
                {
                    client = new Client
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Address = model.Address
                    };
                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();
                }

                // Create a new order for the client
                var order = new Order
                {
                    ClientId = client.Id,
                    DateOfOrder = DateTime.Now,
                    DeadLineDate = model.DeadLineDate,
                    Status = OrderStatus.InProgress // Adjust as needed based on your enum
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Clear cart after order is placed
                CartItems.Clear();

                // Show confirmation message
                TempData["OrderConfirmation"] = "Your order has been made, we will start working on it as soon as possible.";

                return RedirectToAction("OrderConfirmation");
            }
            return View("Order", model);
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        // Clear the cart
        public IActionResult ClearCart()
        {
            CartItems.Clear();
            return RedirectToAction("Index");
        }
    }
}
