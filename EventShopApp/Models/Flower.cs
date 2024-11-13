using EventShopApp.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventShopApp.Models
{
    public class Flower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxFlowerTypeLength, ErrorMessage = "Flower Type cannot be longer than 20 characters.")]
        public required string FlowerType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be higher than zero.")]
        public decimal Price { get; set; }

        [StringLength(ValidationConstants.MaxDescriptionLength, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? Description { get; set; }

        [Required]
        [Range(0, ValidationConstants.MaxFlowerQuantity, ErrorMessage = "Quantity must be between 0 and 1000.")]
        public int FlowerQuantity { get; set; }

        [Url]
        public string? FlowerImageUrl { get; set; }

        public bool IsAvailable => FlowerQuantity > 0;
    }
}
