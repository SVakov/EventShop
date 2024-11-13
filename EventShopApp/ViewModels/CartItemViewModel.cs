using EventShopApp.Enums;

namespace EventShopApp.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public OrderType ItemType { get; set; } // Uses OrderType enum for type safety
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
