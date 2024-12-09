using EventShopApp.Data;
using EventShopApp.Models;
using EventShopApp.Services;
using EventShopApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ArrangementServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly ArrangementService _arrangementService;

    public ArrangementServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _context = new ApplicationDbContext(options);
        _arrangementService = new ArrangementService(_context);
    }

    [Fact]
    public async Task GetAllArrangements_ShouldReturnAllArrangements()
    {
        // Arrange
        _context.ArrangementItems.AddRange(new List<ArrangementItem>
        {
            new ArrangementItem { Id = 1, ArrangementItemType = "Classic", Price = 50, ArrangementItemsQuantity = 10, IsAvailable = true },
            new ArrangementItem { Id = 2, ArrangementItemType = "Modern", Price = 70, ArrangementItemsQuantity = 5, IsAvailable = false }
        });
        await _context.SaveChangesAsync();

        var arrangementsInDb = await _context.ArrangementItems.ToListAsync();
        Assert.NotEmpty(arrangementsInDb);

        // Act
        var result = await _arrangementService.GetAllArrangements();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.ArrangementItemType == "Classic");
        Assert.Contains(result, a => a.ArrangementItemType == "Modern");
    }

    [Fact]
    public async Task GetAllAvailableArrangementsAsync_ShouldReturnOnlyAvailableArrangements()
    {
        // Arrange
        _context.ArrangementItems.AddRange(new List<ArrangementItem>
        {
            new ArrangementItem { Id = 1, ArrangementItemType = "Classic", Price = 50, ArrangementItemsQuantity = 10, IsAvailable = true },
            new ArrangementItem { Id = 2, ArrangementItemType = "Modern", Price = 70, ArrangementItemsQuantity = 5, IsAvailable = false }
        });

        await _context.SaveChangesAsync();

        // Act
        var result = await _arrangementService.GetAllAvailableArrangementsAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.ArrangementItemType == "Classic");
    }

    [Fact]
    public async Task GetArrangementDetailsByIdAsync_ShouldReturnCorrectDetails()
    {
        // Arrange
        _context.ArrangementItems.AddRange(new List<ArrangementItem>
        {
            new ArrangementItem { Id = 1, ArrangementItemType = "Classic", Price = 50, ArrangementItemsQuantity = 10, IsAvailable = true }
        });

        await _context.SaveChangesAsync();

        var exists = await _context.ArrangementItems.AnyAsync(a => a.Id == 1);
        Assert.True(exists, "Arrangement with ID 1 does not exist.");

        // Act
        var result = await _arrangementService.GetArrangementDetailsByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Classic", result.ArrangementItemType);
        Assert.Equal(50, result.Price);
    }

    [Fact]
    public async Task AddArrangement_ShouldAddArrangement()
    {
        // Arrange
        var newArrangement = new ArrangementItem { Id = 3, ArrangementItemType = "Elegant", Price = 60, ArrangementItemsQuantity = 8, IsAvailable = true };

        // Act
        await _arrangementService.AddArrangement(newArrangement);

        // Assert
        var arrangements = await _context.ArrangementItems.ToListAsync();
        Assert.Contains(arrangements, a => a.ArrangementItemType == "Elegant");
    }

    [Fact]
    public async Task SoftDeleteArrangement_ShouldSetIsAvailableToFalse()
    {
        // Arrange
        var arrangement = new ArrangementItem { Id = 1, ArrangementItemType = "Classic", IsAvailable = true };
        _context.ArrangementItems.Add(arrangement);
        await _context.SaveChangesAsync();

        // Debug
        var exists = await _context.ArrangementItems.AnyAsync(a => a.Id == 1);
        Assert.True(exists, "Arrangement with ID 1 does not exist.");

        // Act
        await _arrangementService.SoftDeleteArrangement(1);

        // Assert
        var updatedArrangement = await _context.ArrangementItems.FindAsync(1);
        Assert.NotNull(updatedArrangement);
        Assert.False(updatedArrangement.IsAvailable);
    }

}
