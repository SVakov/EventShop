using EventShopApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItemViewModel> CartItems = new List<CartItemViewModel>();

        // Displays the cart
        public IActionResult Index()
        {
            if (CartItems.Count == 0)
            {
                ViewData["Message"] = "Add items to the cart to proceed to the order.";
            }
            return View(CartItems);
        }

        // Clears the cart
        public IActionResult ClearCart()
        {
            CartItems.Clear();
            return RedirectToAction("Index");
        }

        // Proceed to the order page
        public IActionResult Order()
        {
            if (CartItems.Count == 0)
            {
                return RedirectToAction("Index"); 
            }
            return View(CartItems); 
        }
    }
}
