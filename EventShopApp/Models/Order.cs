using EventShopApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventShopApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required]
        public DateTime DateOfOrder { get; set; }

        [Required]
        public DateTime DeadLineDate { get; set; } 

        [Required]
        public OrderStatus Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
