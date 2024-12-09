using EventShopApp.Areas.Management.Controllers;
using EventShopApp.Data;
using EventShopApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventShopApp.Tests.Controllers
{
    public class ClientsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientsController _controller;

        public ClientsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _controller = new ClientsController(_context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithClients()
        {
            // Arrange
            _context.Clients.AddRange(new List<Client>
            {
                new Client { Id = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "+359894658975" },
                new Client { Id = 2, Name = "Jane Smith", Email = "jane@example.com", PhoneNumber = "+359897658976" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Add_Get_ReturnsView()
        {
            // Act
            var result = _controller.Add();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model); 
        }

        [Fact]
        public async Task Add_Post_ValidClient_RedirectsToIndex()
        {
            // Arrange
            var client = new Client
            {
                Name = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Address = "123 Street"
            };

            // Act
            var result = await _controller.Add(client);

            // Assert
            var addedClient = _context.Clients.FirstOrDefault(c => c.Email == "john@example.com");
            Assert.NotNull(addedClient);
            Assert.Equal("John Doe", addedClient.Name);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Add_Post_InvalidClient_ReturnsViewWithClient()
        {
            // Arrange
            var client = new Client { Name = "", Email = "invalidemail", PhoneNumber = "+359894658975" }; 
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Add(client);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Client>(viewResult.Model);
            Assert.Equal(client, model);

            Assert.Empty(_context.Clients);
        }
    }
}
