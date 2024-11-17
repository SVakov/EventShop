using EventShopApp.ViewModels;

namespace EventShopApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<FlowerViewModel> Flowers { get; set; } = new List<FlowerViewModel>();

        public IEnumerable<ArrangementViewModel> Arrangements { get; set; } = new List<ArrangementViewModel>();

        // Indicates whether the user has the option to register or log in
        public bool ShowRegisterOrLogin { get; set; } = true;
    }
}
