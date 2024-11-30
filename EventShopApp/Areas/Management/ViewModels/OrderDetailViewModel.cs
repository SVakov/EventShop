using EventShopApp.Enums;

namespace EventShopApp.Areas.Management.ViewModels
{
    public class OrderDetailViewModel
    {
        public int? FlowerId { get; set; }
        public int? OrderedFlowerQuantity { get; set; }
        public int? ArrangementItemsId { get; set; }
        public int? OrderedArrangementQuantity { get; set; }
        public OrderType Type { get; set; }  
    }
}
