using EventShopApp.Constants;
using System.ComponentModel.DataAnnotations;

namespace EventShopApp.ViewModels
{
    public class OrderViewModel
    {
        public string Name { get; set; }
        public string ?Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ?Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeadLineDate { get; set; }
        public bool IsPrepayed { get; set; }
    }
}
