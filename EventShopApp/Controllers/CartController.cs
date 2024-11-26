using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services.Interfaces;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CartController(ICartService cartService, SignInManager<IdentityUser> signInManager)
        {
            _cartService = cartService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Redirect to the registered user's cart logic
                return RedirectToAction("Index", "RegisteredUsersCart", new { area = "RegisteredUser" });
            }

            var cartItems = _cartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItemViewModel item)
        {
            _cartService.AddToCart(item);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return Json(new { success = true });
        }

        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            if (_signInManager.IsSignedIn(User))
            {
                // Redirect to the registered user's order logic
                return RedirectToAction("Order", "RegisteredUsersCart", new { area = "RegisteredUser" });
            }

            return View(new OrderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOrder(OrderViewModel model)
        {
            var success = await _cartService.SubmitOrder(model);

            if (success)
            {
                TempData["OrderConfirmation"] = "Your order has been made, we will start working on it as soon as possible.";
                return RedirectToAction("OrderConfirmation");
            }

            return View("Order", model);
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
