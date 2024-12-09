using EventShopApp.Areas.Management.Controllers;
using EventShopApp.Models;
using EventShopApp.Services;
using EventShopApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventShopApp.Tests.Areas.Management
{
    public class ArrangementsControllerTests
    {
        private readonly Mock<IArrangementService> _arrangementServiceMock;
        private readonly ArrangementsController _controller;

        public ArrangementsControllerTests()
        {
            _arrangementServiceMock = new Mock<IArrangementService>();
            _controller = new ArrangementsController(_arrangementServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsFilteredAvailableArrangements()
        {
            // Arrange
            var arrangements = new List<ArrangementItem>
    {
                new ArrangementItem { Id = 1, IsAvailable = true, ArrangementItemType = "Bouquet" },
                new ArrangementItem { Id = 2, IsAvailable = false, ArrangementItemType = "Centerpiece" }
    };
            _arrangementServiceMock.Setup(s => s.GetAllArrangements())
                .ReturnsAsync(arrangements);

            // Act
            var result = await _controller.Index(filter: "available");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ArrangementItem>>(viewResult.Model);
            Assert.Single(model);
            Assert.True(model.First().IsAvailable);
        }


        [Fact]
        public async Task Index_SortsArrangementsByPriceAsc()
        {
            // Arrange
            var arrangements = new List<ArrangementItem>
            {
                new ArrangementItem { Id = 1, Price = 20, ArrangementItemType = "Bouquet" },
                new ArrangementItem { Id = 2, Price = 10, ArrangementItemType = "Centerpiece" }
            };
            _arrangementServiceMock.Setup(s => s.GetAllArrangements())
                .ReturnsAsync(arrangements);

            // Act
            var result = await _controller.Index(sortOrder: "price-asc");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ArrangementItem>>(viewResult.Model);
            Assert.Equal(10, model.First().Price);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            _arrangementServiceMock.Setup(s => s.GetArrangementById(It.IsAny<int>()))
                .ReturnsAsync((ArrangementItem)null);

            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_SoftDeletesArrangement()
        {
            // Arrange
            _arrangementServiceMock.Setup(s => s.SoftDeleteArrangement(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task BringBack_RestoresArrangement()
        {
            // Arrange
            var arrangementId = 1;
            _arrangementServiceMock.Setup(s => s.BringBackArrangement(arrangementId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.BringBack(arrangementId);

            // Assert
            _arrangementServiceMock.Verify(s => s.BringBackArrangement(arrangementId), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

    }
}
