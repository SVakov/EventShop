using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class FlowerServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly FlowerService _flowerService;

    public FlowerServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //unique database for each test
            .Options;

        _context = new ApplicationDbContext(options);
        _flowerService = new FlowerService(_context);
    }

    [Fact]
    public async Task GetAllFlowers_ShouldReturnAllFlowers()
    {
        // Arrange
        _context.Flowers.AddRange(new List<Flower>
        {
            new Flower { Id = 1, FlowerType = "Rose", Price = 10, FlowerQuantity = 100, IsAvailable = true },
            new Flower { Id = 2, FlowerType = "Tulip", Price = 5, FlowerQuantity = 50, IsAvailable = false }
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _flowerService.GetAllFlowers();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, f => f.FlowerType == "Rose");
    }

    [Fact]
    public async Task AddFlower_ShouldAddFlower()
    {
        // Arrange
        var newFlower = new Flower { Id = 3, FlowerType = "Lily", Price = 7, FlowerQuantity = 30, IsAvailable = true };

        // Act
        await _flowerService.AddFlower(newFlower);

        // Assert
        var flowers = await _context.Flowers.ToListAsync();
        Assert.Contains(flowers, f => f.FlowerType == "Lily");
    }

    [Fact]
    public async Task GetFlowerDetailsByIdAsync_ShouldReturnCorrectFlowerDetails()
    {
        // Arrange
        _context.Flowers.Add(new Flower { Id = 1, FlowerType = "Rose", Price = 10, FlowerQuantity = 100, IsAvailable = true });
        await _context.SaveChangesAsync();

        // Act
        var result = await _flowerService.GetFlowerDetailsByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Rose", result.FlowerType);
    }
}
