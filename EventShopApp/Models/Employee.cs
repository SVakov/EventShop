using EventShopApp.Constants;
using EventShopApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace EventShopApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxNameLength, ErrorMessage = ErrorMessages.MaxNameLength)]
        public required string Name { get; set; }

        [StringLength(ValidationConstants.MaxSurnameLength, ErrorMessage = ErrorMessages.MaxSurnameLength)]
        public string? Surname { get; set; }

        [Required]
        public required EmployeeRole Role { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxPhoneNumberLength, ErrorMessage = ErrorMessages.MaxPhoneNumberLength)]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(ValidationConstants.MaxEmailLength, ErrorMessage = ErrorMessages.MaxEmailLength)]
        [EmailAddress(ErrorMessage = ErrorMessages.InvalidEmailFormat)]
        public required string Email { get; set; }

        [Required]
        public required DateTime HireDate { get; set; }

        public bool IsFired { get; set; }
    }
}
