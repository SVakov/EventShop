﻿using EventShopApp.Enums;

namespace EventShopApp.ViewModels
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public OrderType ItemType { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
