using EventShopApp.Areas.Management.Controllers;
using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services;
using EventShopApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventShopApp.Tests.Areas.Management
{
    public class FlowersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly FlowerService _flowerService;
        private readonly FlowersController _controller;

        public FlowersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _flowerService = new FlowerService(_context);
            _controller = new FlowersController(_flowerService);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFoundForInvalidId()
        {
            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_SoftDeletesFlower()
        {
            // Arrange
            var flower = new Flower { Id = 1, IsAvailable = true, FlowerType = "Rose" };
            _context.Flowers.Add(flower);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var deletedFlower = await _context.Flowers.FindAsync(1);
            Assert.False(deletedFlower.IsAvailable);
        }

        [Fact]
        public async Task BringBack_RestoresFlower()
        {
            // Arrange
            var flower = new Flower { Id = 1, IsAvailable = false, FlowerType = "Rose" };
            _context.Flowers.Add(flower);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.BringBack(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var restoredFlower = await _context.Flowers.FindAsync(1);
            Assert.True(restoredFlower.IsAvailable);
        }
    }
}
