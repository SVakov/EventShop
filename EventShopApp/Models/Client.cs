using EventShopApp.Constants;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace EventShopApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxNameLength, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public required string Name { get; set; }

        [StringLength(ValidationConstants.MaxSurnameLength, ErrorMessage = "Surname cannot be longer than 50 characters.")]
        public string? Surname { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxPhoneNumberLength, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxEmailLength, ErrorMessage = "Email cannot be longer than 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public required string Email { get; set; }

        [StringLength(ValidationConstants.MaxAddressLength, ErrorMessage = "Address cannot be longer than 50 characters.")]
        public string? Address { get; set; }
    }
}
