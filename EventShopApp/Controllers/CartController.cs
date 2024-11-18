using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services.Interfaces;
using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
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
