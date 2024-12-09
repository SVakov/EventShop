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
        [StringLength(ValidationConstants.MaxFlowerTypeLength, ErrorMessage = ErrorMessages.MaxFlowerTypeLength)]
        public required string FlowerType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = ErrorMessages.PriceMustBePositive)]
        public decimal Price { get; set; }

        [StringLength(ValidationConstants.MaxDescriptionLength, ErrorMessage = ErrorMessages.MaxDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        [Range(0, ValidationConstants.MaxFlowerQuantity, ErrorMessage = ErrorMessages.FlowerQuantityRange)]
        public int FlowerQuantity { get; set; }

        [Url]
        public string? FlowerImageUrl { get; set; }

        private bool _isAvailable;

        public bool IsAvailable
        {
            get => _isAvailable;  
            set => _isAvailable = value; 
        }
    }
}
