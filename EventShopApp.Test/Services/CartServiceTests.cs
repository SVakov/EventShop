using EventShopApp.Data;
using EventShopApp.Enums;
using EventShopApp.Models;
using EventShopApp.Services.Implementation;
using EventShopApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
        _context = new ApplicationDbContext(options);
        _cartService = new CartService(_context);
    }

    [Fact]
    public void AddToCart_ShouldAddNewItemToCart()
    {
        // Arrange
        var item = new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 };

        // Act
        _cartService.AddToCart(item);
        var cartItems = _cartService.GetCartItems();

        // Assert
        Assert.Single(cartItems);
        Assert.Equal(2, cartItems.First().Quantity);
    }

    [Fact]
    public void AddToCart_ShouldIncreaseQuantityIfItemExists()
    {
        // Arrange
        var item = new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 };
        _cartService.AddToCart(item);

        // Act
        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 4 });
        var cartItems = _cartService.GetCartItems();

        // Assert
        Assert.Single(cartItems);
        Assert.Equal(6, cartItems.First().Quantity);
    }

    [Fact]
    public void RemoveFromCart_ShouldRemoveItemById()
    {
        // Arrange
        var item = new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 };
        _cartService.AddToCart(item);

        // Act
        _cartService.RemoveFromCart(1);
        var cartItems = _cartService.GetCartItems();

        // Assert
        Assert.Empty(cartItems);
    }

    [Fact]
    public void ClearCart_ShouldRemoveAllItems()
    {
        // Arrange
        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 });
        _cartService.AddToCart(new CartItemViewModel { Id = 2, ItemType = OrderType.Arrangement, Quantity = 1 });

        // Act
        _cartService.ClearCart();
        var cartItems = _cartService.GetCartItems();

        // Assert
        Assert.Empty(cartItems);
    }

    [Fact]
    public async Task SubmitOrder_ShouldReturnFalseIfCartIsEmpty()
    {
        // Arrange
        var orderViewModel = new OrderViewModel
        {
            Email = "test@example.com",
            Name = "John Doe"
        };

        // Act
        var result = await _cartService.SubmitOrder(orderViewModel);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SubmitOrder_ShouldAddClientIfNotExists()
    {
        // Arrange
        var orderViewModel = new OrderViewModel
        {
            Email = "newclient@example.com",
            Name = "Jane Doe",
            PhoneNumber = "+359897468597"
        };

        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 1 });

        // Act
        await _cartService.SubmitOrder(orderViewModel);

        // Assert
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == "newclient@example.com");
        Assert.NotNull(client);
        Assert.Equal("Jane Doe", client.Name);
    }

    [Fact]
    public async Task SubmitOrder_ShouldAddOrderWithDetails()
    {
        // Arrange
        _context.Clients.Add(new Client { Id = 1, Name = "John Doe", Email = "test@example.com", PhoneNumber = "+359879564897" });
        _context.Flowers.Add(new Flower { Id = 1, FlowerType = "Rose", FlowerQuantity = 5 });
        await _context.SaveChangesAsync();

        var orderViewModel = new OrderViewModel
        {
            Email = "test@example.com",
            Name = "John Doe"
        };

        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 2 });

        // Act
        var result = await _cartService.SubmitOrder(orderViewModel);

        // Assert
        Assert.True(result);
        var order = await _context.Orders.FirstOrDefaultAsync();
        var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync();
        var flower = await _context.Flowers.FirstOrDefaultAsync();

        Assert.NotNull(order);
        Assert.NotNull(orderDetail);
        Assert.Equal(3, flower.FlowerQuantity);
    }
}
