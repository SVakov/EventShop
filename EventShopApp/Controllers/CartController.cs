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
            // Find existing item based on Id and ItemType
            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id && i.ItemType == item.ItemType);

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

                // Iterate over cart items
                foreach (var cartItem in CartItems)
                {
                    if (cartItem.ItemType == OrderType.Flower)
                    {
                        // Decrease stock for flowers
                        var flower = _context.Flowers.FirstOrDefault(f => f.Id == cartItem.Id);
                        if (flower != null && flower.FlowerQuantity >= cartItem.Quantity)
                        {
                            flower.FlowerQuantity -= cartItem.Quantity;
                        }

                        // Add to OrderDetails for flowers
                        //var orderDetail = new OrderDetail
                       // {
                        //    OrderId = order.Id,
                        //    FlowerId = flower?.Id ?? 0, // Flower is mandatory
                        //    OrderedFlowerQuantity = cartItem.Quantity,
                        //    Type = OrderType.Flower,
                        //    ArrangementItemsId = null, // Not applicable for flowers
                        //    IsPrepayed = false
                        //};
                        //_context.OrderDetails.Add(orderDetail);
                    }
                    else if (cartItem.ItemType == OrderType.Arrangement)
                    {
                        // Decrease stock for arrangements
                        var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == cartItem.Id);
                        if (arrangement != null && arrangement.ArrangementItemsQuantity >= cartItem.Quantity)
                        {
                            arrangement.ArrangementItemsQuantity -= cartItem.Quantity;
                        }

                        // Add to OrderDetails for arrangements
                        //var orderDetail = new OrderDetail
                       // {
                       //     OrderId = order.Id,
                       //     FlowerId = 0, // Not applicable for arrangements
                       //     OrderedFlowerQuantity = 0, // Not applicable for arrangements
                       //     Type = OrderType.Arrangement,
                       //     ArrangementItemsId = arrangement?.Id,
                       //     IsPrepayed = false
                       // };
                       // _context.OrderDetails.Add(orderDetail);
                    }
                }

                await _context.SaveChangesAsync();

                // Clear cart after order is placed
                CartItems.Clear();

                // Show confirmation message
                TempData["OrderConfirmation"] = "Your order has been made, we will start working on it as soon as possible.";

                return RedirectToAction("OrderConfirmation");
            }

            return View("Order", model);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var item = CartItems.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                CartItems.Remove(item);
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
