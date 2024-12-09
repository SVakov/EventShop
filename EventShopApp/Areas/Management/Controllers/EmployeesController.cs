using EventShopApp.Areas.Management.ViewModels;
using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventShopApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Policy = "ManagementAccess")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        public IActionResult Add()
        {
            Console.WriteLine("Add action triggered"); // Debugging
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Employee employee, string TemporaryPassword)
        {

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                return View(employee);
            }

            if (_context.Employees.Any(e => e.Email == employee.Email))
            {
                ModelState.AddModelError(string.Empty, "An employee with this email already exists.");
                return View(employee);
            }

            employee.HireDate = DateTime.UtcNow;
            employee.IsFired = false;

            var user = new IdentityUser { UserName = employee.Email, Email = employee.Email };
            var result = await _userManager.CreateAsync(user, TemporaryPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }
                return View(employee);
            }

            var roleName = employee.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);

            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine($"Assigned roles: {string.Join(", ", roles)}");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = employee.Id,
                role = employee.Role.ToString(),
                isFired = employee.IsFired
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EmployeeEditViewModel updatedFields)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var existingEmployee = await _context.Employees.FindAsync(updatedFields.Id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            if (Enum.TryParse(updatedFields.Role, out EmployeeRole role))
            {
                existingEmployee.Role = role;
            }
            else
            {
                return BadRequest(new { success = false, errors = new[] { "Invalid role value." } });
            }

            existingEmployee.IsFired = updatedFields.IsFired;

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }







    }
}
