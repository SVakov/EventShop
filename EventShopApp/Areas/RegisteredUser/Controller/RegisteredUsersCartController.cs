using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services.Interfaces;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventShopApp.Areas.RegisteredUsers.Controllers
{
    [Area("RegisteredUser")]
    [Authorize]
    public class RegisteredUsersCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisteredUsersCartController(ICartService cartService, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _cartService = cartService;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return Ok();
        }

        public IActionResult Order()
        {
            var userEmail = _userManager.GetUserName(User);

           

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["OrderError"] = "Error: Could not fetch user email.";
                return RedirectToAction("Index", "Home"); // Redirect if user email is not found
            }

            var orderModel = new OrderViewModel
            {
                Email = userEmail // Pre-fill the email
            };

            Console.WriteLine($"Order model being passed to view: {orderModel.Email}");

            return View(orderModel); // Use the Order view for registered users
        }


        [HttpPost]
        public async Task<IActionResult> SubmitOrder(OrderViewModel model)
        {

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {error.ErrorMessage}");
                }
                return Json(new { success = false, message = "Missing required fields." });
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if Client exists
                    var client = _context.Clients.FirstOrDefault(c => c.Email == model.Email);

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

                    // Create Order
                    var order = new Order
                    {
                        ClientId = client.Id,
                        DateOfOrder = DateTime.Now,
                        DeadLineDate = model.DeadLineDate,
                        Status = OrderStatus.InProgress
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    // Handle Cart Items
                    foreach (var cartItem in _cartService.GetCartItems())
                    {
                        if (cartItem.ItemType == OrderType.Flower)
                        {
                            var flower = _context.Flowers.FirstOrDefault(f => f.Id == cartItem.Id);
                            if (flower != null && flower.FlowerQuantity >= cartItem.Quantity)
                            {
                                flower.FlowerQuantity -= cartItem.Quantity;

                                _context.OrderDetails.Add(new OrderDetail
                                {
                                    OrderId = order.Id,
                                    FlowerId = flower.Id,
                                    OrderedFlowerQuantity = cartItem.Quantity,
                                    Type = OrderType.Flower,
                                    ArrangementItemsId = null,
                                    OrderedArrangementQuantity = null,
                                    IsPrepayed = false
                                });
                            }
                        }
                        else if (cartItem.ItemType == OrderType.Arrangement)
                        {
                            var arrangement = _context.ArrangementItems.FirstOrDefault(a => a.Id == cartItem.Id);
                            if (arrangement != null && arrangement.ArrangementItemsQuantity >= cartItem.Quantity)
                            {
                                arrangement.ArrangementItemsQuantity -= cartItem.Quantity;

                                _context.OrderDetails.Add(new OrderDetail
                                {
                                    OrderId = order.Id,
                                    FlowerId = null,
                                    OrderedFlowerQuantity = null,
                                    Type = OrderType.Arrangement,
                                    ArrangementItemsId = arrangement.Id,
                                    OrderedArrangementQuantity = cartItem.Quantity,
                                    IsPrepayed = false
                                });
                            }
                        }
                    }

                    // Save changes and commit transaction
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Clear the cart after order is placed
                    _cartService.GetCartItems().Clear();

                    TempData["OrderConfirmation"] = "Your order has been made, we will start working on it as soon as possible.";
                    return Json(new { success = true, redirectUrl = Url.Action("OrderConfirmation", "RegisteredUsersCart", new { area = "RegisteredUser" }) });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during SubmitOrder: {ex.Message}");
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "An error occurred while processing your order. Please try again." });
                }
            }
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
