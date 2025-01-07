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
        // Arrange: Seed the database with a flower
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            FlowerQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "Red Roses"
        };
        _context.Flowers.Add(flower);
        _context.SaveChanges();

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
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            FlowerQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "Red Roses"
        };
        _context.Flowers.Add(flower);
        _context.SaveChanges();
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
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            FlowerQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "Red Roses"
        };
        _context.Flowers.Add(flower);
        _context.SaveChanges();
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
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            FlowerQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "Red Roses"
        };
        _context.Flowers.Add(flower);
        var arrangement = new ArrangementItem
        {
            Id = 2,
            ArrangementItemType = "Birthday Bouquet",
            ArrangementItemsQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "For Birthdays"
        };
        _context.ArrangementItems.Add(arrangement);
        _context.SaveChanges();

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
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            FlowerQuantity = 10, // Ensure sufficient stock
            Price = 5.0m,
            Description = "Red Roses"
        };
        _context.Flowers.Add(flower);
        _context.SaveChanges();
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
        _context.Flowers.Add(new Flower { Id = 1, FlowerType = "Rose", FlowerQuantity = 3 });
        await _context.SaveChangesAsync();

        var orderViewModel = new OrderViewModel
        {
            Email = "test@example.com",
            Name = "John Doe"
        };

        // Act: Add item to cart
        _cartService.AddToCart(new CartItemViewModel { Id = 1, ItemType = OrderType.Flower, Quantity = 1 });

        // Verify stock deduction after AddToCart
        var flowerAfterAddToCart = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == 1);
        Assert.Equal(2, flowerAfterAddToCart.FlowerQuantity);

        // Act: Submit the order
        var result = await _cartService.SubmitOrder(orderViewModel);

        // Assert
        Assert.True(result);

        var order = await _context.Orders.FirstOrDefaultAsync();
        Assert.NotNull(order); // Ensure order was created

        var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync();
        Assert.NotNull(orderDetail); // Ensure order detail was created
        Assert.Equal(1, orderDetail.OrderedFlowerQuantity); // Verify quantity in order detail

        var flowerAfterOrder = await _context.Flowers.FirstOrDefaultAsync();
        Assert.Equal(2, flowerAfterOrder.FlowerQuantity); // Stock should remain 3
    }
}
