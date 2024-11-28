//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;

//namespace EventShopApp.Areas.Management.Controllers
//{
//    [Area("Management")]
//    [Authorize(Policy = "ManagementAccess")]
//    public class RoleTestController : Controller
//    {
//        public IActionResult Index()
//        {
//            var email = User.Identity?.Name;
//            if (email == "vakovslavcho@gmail.com")
//            {
//                return Content($"Logged in as Owner with email: {email}");
//            }

//            return Content("Not logged in as Owner.");
//        }
//    }
//}
