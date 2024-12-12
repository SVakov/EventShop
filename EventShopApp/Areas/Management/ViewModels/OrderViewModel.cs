using System;
using System.Collections.Generic;
using EventShopApp.Models;
using EventShopApp.Enums;
using EventShopApp.Constants;
using System.ComponentModel.DataAnnotations;

namespace EventShopApp.Areas.Management.ViewModels
{
    public class OrderViewModel
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string? ClientSurname { get; set; }
        public string ClientEmail { get; set; }

        public string? ClientAddress { get; set; }
        public string ClientPhoneNumber { get; set; }
        public DateTime DateOfOrder { get; set; }

        public DateTime DeadLineDate { get; set; }

        
        public bool IsPrepayed { get; set; }

        
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }
}
