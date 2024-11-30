using EventShopApp.Enums;
using System;
using System.Collections.Generic;

namespace EventShopApp.Areas.Management.ViewModels
{
    public class OrderViewModel
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNumber { get; set; }
        public DateTime DeadLineDate { get; set; }
        public bool IsPrepayed { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int FlowerId { get; set; }
        public int? OrderedFlowerQuantity { get; set; }
        public int? ArrangementItemsId { get; set; }
        public int? OrderedArrangementQuantity { get; set; }
        public OrderType Type { get; set; }
    }
}
