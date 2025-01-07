using EventShopApp.ViewModels;

namespace EventShopApp.Services.Interfaces
{
    public interface ICartService
    {
        List<CartItemViewModel> GetCartItems();
        void AddToCart(CartItemViewModel item);
        void RemoveFromCart(int id);
        void ClearCart();
        Task<bool> SubmitOrder(OrderViewModel model);
        void CleanupStaleCartItems(TimeSpan timeout);
    }
}
