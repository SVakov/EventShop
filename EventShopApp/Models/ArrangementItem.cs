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
        [StringLength(ValidationConstants.MaxArrangementItemTypeLength, ErrorMessage = ErrorMessages.MaxArrangementItemTypeLength)]
        public required string ArrangementItemType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = ErrorMessages.PriceMustBePositive)]
        public decimal Price { get; set; }

        [StringLength(ValidationConstants.MaxDescriptionLength, ErrorMessage = ErrorMessages.MaxDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        [Range(0, ValidationConstants.MaxArrangementItemsQuantity, ErrorMessage = ErrorMessages.ArrangementQuantityRange)]
        public int ArrangementItemsQuantity { get; set; }

        [Url]
        public string? ArrangementItemImageUrl { get; set; }

        private bool _isAvailable;

        public bool IsAvailable
        {
            get => _isAvailable;  
            set => _isAvailable = value;  
        }
    }
}
