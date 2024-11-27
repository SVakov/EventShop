namespace EventShopApp.Areas.RegisteredUser.ViewModels
{

    public class ProfileViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }

        public List<OrderDetailsViewModel> Orders { get; set; } = new List<OrderDetailsViewModel>();
    }
}
