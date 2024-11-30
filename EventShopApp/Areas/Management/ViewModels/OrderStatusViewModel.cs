using EventShopApp.Enums;

namespace EventShopApp.Areas.Management.ViewModels
{
    public class OrderStatusViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
