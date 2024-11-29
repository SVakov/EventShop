using EventShopApp.Enums;

namespace EventShopApp.Areas.Management.ViewModels
{
    public class EmployeeEditViewModel
    {    
            public int Id { get; set; }
            public string Role { get; set; } // Use string instead of EmployeeRole
            public bool IsFired { get; set; }
    } 
}
