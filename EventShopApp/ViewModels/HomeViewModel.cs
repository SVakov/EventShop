using EventShopApp.Models;

namespace EventShopApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Flower> Flowers { get; set; } = new List<Flower>();

        public IEnumerable<ArrangementItem> Arrangements { get; set; } = new List<ArrangementItem>();

        // Indicates whether the user has the option to register or log in
        public bool ShowRegisterOrLogin { get; set; } = true;
    }
}
