using EventShopApp.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace EventShopApp.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxNameLength, ErrorMessage = ErrorMessages.MaxNameLength)]
        public required string Name { get; set; }

        [StringLength(ValidationConstants.MaxSurnameLength, ErrorMessage = ErrorMessages.MaxSurnameLength)]
        public string? Surname { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxPhoneNumberLength, ErrorMessage = ErrorMessages.MaxPhoneNumberLength)]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxEmailLength, ErrorMessage = ErrorMessages.MaxEmailLength)]
        [EmailAddress(ErrorMessage = ErrorMessages.InvalidEmailFormat)]
        public required string Email { get; set; }

        [StringLength(ValidationConstants.MaxAddressLength, ErrorMessage = ErrorMessages.MaxAddressLength)]
        public string? Address { get; set; }

    }
}
