using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EventShopApp.Areas.RegisteredUser.Controllers;
using EventShopApp.Services.Interfaces;
using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventShopApp.Areas.Management.ViewModels;
using EventShopApp.Areas.RegisteredUsers.Controllers;
using EventShopApp.Enums;
using System.Linq.Expressions;
using OrderViewModel = EventShopApp.ViewModels.OrderViewModel;
using EventShopApp.Services.Implementation;
using Microsoft.AspNetCore.Http;

public class RegisteredUsersCartControllerTests
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RegisteredUsersCartController _controller;

    public RegisteredUsersCartControllerTests()
    {
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
         .Options;
        _context = new ApplicationDbContext(options);

       
        _cartService = new CartService(_context);

        
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        _userManager = new UserManager<IdentityUser>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        
        _controller = new RegisteredUsersCartController(
            _cartService,
            _userManager,
            _context
        );
    }

    [Fact]
    public void Index_ReturnsViewWithCartItems()
    {
        // Arrange
        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 });
        _cartService.AddToCart(new CartItemViewModel { Id = 2, ItemType = OrderType.Arrangement, Quantity = 1 });

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CartItemViewModel>>(viewResult.Model);
        Assert.Equal(2, model.Count());
        Assert.Contains(model, ci => ci.Id == 1 && ci.Quantity == 2);
        Assert.Contains(model, ci => ci.Id == 2 && ci.Quantity == 1);
    }

    [Fact]
    public void ClearCart_ClearsCartAndReturnsOk()
    {
        // Arrange
        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 });
        _cartService.AddToCart(new CartItemViewModel { Id = 2, ItemType = OrderType.Arrangement, Quantity = 1 });

        // Act
        var result = _controller.ClearCart();

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        var cartItems = _cartService.GetCartItems();
        Assert.Empty(cartItems);
    }

    [Fact]
    public void OrderConfirmation_ReturnsView()
    {
        // Act
        var result = _controller.OrderConfirmation();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }
}
