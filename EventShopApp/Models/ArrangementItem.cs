using EventShopApp.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventShopApp.Models
{
    public class ArrangementItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxArrangementItemTypeLength, ErrorMessage = "Arrangement Type cannot be longer than 50 characters.")]
        public required string ArrangementItemType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be higher than zero.")]
        public decimal Price { get; set; }

        [StringLength(ValidationConstants.MaxDescriptionLength, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? Description { get; set; }

        [Required]
        [Range(0, ValidationConstants.MaxArrangementItemsQuantity, ErrorMessage = "Quantity must be between 0 and 500.")]
        public int ArrangementItemsQuantity { get; set; }

        [Url]
        public string? ArrangementItemImageUrl { get; set; }

        public bool IsAvailable => ArrangementItemsQuantity > 0;
    }
}
