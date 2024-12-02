namespace EventShopApp.ViewModels
{
    public class FlowerViewModel
    {
        public int Id { get; set; }
        public string FlowerType { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string FlowerImageUrl { get; set; }

        public int? FlowerQuantity { get; set; }

        
    }
}
