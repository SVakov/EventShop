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

        private bool ValidateOrderDetail(OrderDetail detail)
        {
            if (detail.Type == OrderType.Flower)
            {
                return detail.FlowerId.HasValue && detail.OrderedFlowerQuantity.HasValue && detail.OrderedFlowerQuantity.Value > 0;
            }
            else if (detail.Type == OrderType.Arrangement)
            {
                return detail.ArrangementItemsId.HasValue && detail.OrderedArrangementQuantity.HasValue && detail.OrderedArrangementQuantity.Value > 0;
            }
            return false; // Invalid type or missing required fields
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
                    Status = OrderStatus.InProgress
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create OrderDetails for each item in the cart
                foreach (var cartItem in CartItems)
                {
                    if (cartItem.ItemType == OrderType.Flower)
                    {
                        // Handle flowers
                        var flower = _context.Flowers.FirstOrDefault(f => f.Id == cartItem.Id);
                        if (flower != null && flower.FlowerQuantity >= cartItem.Quantity)
                        {
                            flower.FlowerQuantity -= cartItem.Quantity; // Decrease stock

                            var orderDetail = new OrderDetail
                            {
                                OrderId = order.Id,
                                FlowerId = flower.Id,
                                OrderedFlowerQuantity = cartItem.Quantity,
                                Type = OrderType.Flower,
                                ArrangementItemsId = null,
                                OrderedArrangementQuantity = null,
                                IsPrepayed = false
                            };

                            // Validate and add OrderDetail
                            if (ValidateOrderDetail(orderDetail))
                            {
                                _context.OrderDetails.Add(orderDetail);
                            }
                        }
                    }
                    else if (cartItem.ItemType == OrderType.Arrangement)
                    {
                        // Handle arrangements
                        var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == cartItem.Id);
                        if (arrangement != null && arrangement.ArrangementItemsQuantity >= cartItem.Quantity)
                        {
                            arrangement.ArrangementItemsQuantity -= cartItem.Quantity; // Decrease stock

                            var orderDetail = new OrderDetail
                            {
                                OrderId = order.Id,
                                FlowerId = null,
                                OrderedFlowerQuantity = null,
                                Type = OrderType.Arrangement,
                                ArrangementItemsId = arrangement.Id,
                                OrderedArrangementQuantity = cartItem.Quantity,
                                IsPrepayed = false
                            };

                            // Validate and add OrderDetail
                            if (ValidateOrderDetail(orderDetail))
                            {
                                _context.OrderDetails.Add(orderDetail);
                            }
                        }
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
