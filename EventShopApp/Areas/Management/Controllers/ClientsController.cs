using EventShopApp.Models;
using EventShopApp.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventShopApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventShopApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index page - List of clients
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }

        // Add new client form
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Client client)
        {
            // Log the received client data
            Console.WriteLine($"Name: {client.Name}, Email: {client.Email}, Phone: {client.PhoneNumber}, Address: {client.Address}");

            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log model state errors for further diagnosis
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  // Or use logging
                }

                // Return the form with validation errors
                return View(client);
            }
        }


    }
}
