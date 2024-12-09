using EventShopApp.Areas.Management.Controllers;
using EventShopApp.Areas.Management.ViewModels;
using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventShopApp.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new OrdersController(_context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, Client = new Client { Name = "John", Email = "john@example.com", PhoneNumber = "+43 68110569759" }, Status = OrderStatus.Pending },
                new Order { Id = 2, Client = new Client { Name = "Jane", Email = "jane@example.com", PhoneNumber = "+43 68110569768" }, Status = OrderStatus.Completed }
            };
            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Order>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Add_Get_ReturnsViewWithFlowerAndArrangementData()
        {
            // Arrange
            var flowers = new List<Flower>
            {
                new Flower { Id = 1, FlowerType = "Rose" },
                new Flower { Id = 2, FlowerType = "Tulip" }
            };
            var arrangements = new List<ArrangementItem>
            {
                new ArrangementItem { Id = 1, ArrangementItemType = "Bouquet" },
                new ArrangementItem { Id = 2, ArrangementItemType = "Basket" }
            };

            _context.Flowers.AddRange(flowers);
            _context.ArrangementItems.AddRange(arrangements);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Add();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(flowers, viewResult.ViewData["Flowers"]);
            Assert.Equal(arrangements, viewResult.ViewData["Arrangements"]);
        }

        [Fact]
        public async Task Add_Post_ValidOrder_RedirectsToIndex()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                ClientName = "John",
                ClientEmail = "john@example.com",
                ClientPhoneNumber = "1234567890",
                DeadLineDate = DateTime.Now.AddDays(5),
                OrderDetails = new List<OrderDetailViewModel>
                {
                    new OrderDetailViewModel { FlowerId = 1, OrderedFlowerQuantity = 5 }
                }
            };

            var flowers = new List<Flower>
            {
                new Flower { Id = 1, FlowerType = "Rose", FlowerQuantity = 10 }
            };
            _context.Flowers.AddRange(flowers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Add(orderViewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var addedOrder = _context.Orders.Include(o => o.Client).FirstOrDefault();
            Assert.NotNull(addedOrder);
            Assert.Equal("John", addedOrder.Client.Name);
        }

        [Fact]
        public async Task Add_Post_InvalidOrder_ReturnsViewWithModel()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                ClientName = "John"
            };

            _controller.ModelState.AddModelError("ClientEmail", "The Email field is required.");

            // Act
            var result = await _controller.Add(orderViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(orderViewModel, viewResult.Model);
            Assert.Empty(_context.Orders);
        }

        [Fact]
        public async Task UpdateOrderStatus_ValidOrder_UpdatesStatus()
        {
            // Arrange
            var order = new Order { Id = 1, Status = OrderStatus.Pending };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.UpdateOrderStatus(1, "Completed");

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value));

            var updatedOrder = _context.Orders.First(o => o.Id == 1);
            Assert.Equal(OrderStatus.Completed, updatedOrder.Status);
        }
    }
}
