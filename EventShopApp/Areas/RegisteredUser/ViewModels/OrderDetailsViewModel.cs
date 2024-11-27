public class OrderDetailsViewModel
{
    public string ItemType { get; set; } // Flower or Arrangement
    public string ItemName { get; set; } // Name of the item
    public int Quantity { get; set; }
    public DateTime DateOfOrder { get; set; }
    public DateTime DeadLineDate { get; set; }
    public string Status { get; set; } // In Progress, Completed, etc.
}
