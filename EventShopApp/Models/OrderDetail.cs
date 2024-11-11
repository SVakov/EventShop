using EventShopApp.Constants;
using EventShopApp.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventShopApp.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } 

        [ForeignKey("OrderId")]
        public Order Order { get; set; } 

        [Required]
        public int FlowerId { get; set; } 

        [ForeignKey("FlowerId")]
        public Flower Flower { get; set; } 

        [Required]
        public int OrderedFlowerQuantity { get; set; } 

        [Required]
        public OrderType Type { get; set; } 

        public int? ArrangementItemsId { get; set; } 

        [ForeignKey("ArrangementItemsId")]
        public ArrangementItem? ArrangementItem { get; set; } 

        [Required]
        public bool IsPrepayed { get; set; } 
    }
}
