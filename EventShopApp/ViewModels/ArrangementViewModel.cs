namespace EventShopApp.ViewModels
{
    public class ArrangementViewModel
    {
        public int Id { get; set; }
        public string ArrangementItemType { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ArrangementItemImageUrl { get; set; }
        public int ArrangementItemsQuantity { get; set; }
    }
}
