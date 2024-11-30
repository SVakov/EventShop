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

        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Client client)
        {
            Console.WriteLine($"Name: {client.Name}, Email: {client.Email}, Phone: {client.PhoneNumber}, Address: {client.Address}");

            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  
                }

                return View(client);
            }
        }


    }
}
