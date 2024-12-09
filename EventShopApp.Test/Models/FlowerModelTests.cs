using EventShopApp.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class FlowerModelTests
{
    [Fact]
    public void Flower_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            Price = 10.5m,
            Description = "A beautiful red rose.",
            FlowerQuantity = 50,
            FlowerImageUrl = "https://example.com/rose.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(flower);

        // Act
        var isValid = Validator.TryValidateObject(flower, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "The FlowerType field is required.")]
    [InlineData("ThisIsAVeryLongFlowerTypeName", "Flower Type cannot be longer than 20 characters.")]
    public void Flower_InvalidFlowerType_ProducesValidationError(string flowerType, string expectedErrorMessage)
    {
        // Arrange
        var flower = new Flower
        {
            Id = 1,
            FlowerType = flowerType,
            Price = 10.5m,
            Description = "A beautiful red rose.",
            FlowerQuantity = 50,
            FlowerImageUrl = "https://example.com/rose.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(flower);

        // Act
        var isValid = Validator.TryValidateObject(flower, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(-1, "Price must be higher than zero.")]
    [InlineData(0, "Price must be higher than zero.")]
    public void Flower_InvalidPrice_ProducesValidationError(decimal price, string expectedErrorMessage)
    {
        // Arrange
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            Price = price,
            Description = "A beautiful red rose.",
            FlowerQuantity = 50,
            FlowerImageUrl = "https://example.com/rose.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(flower);

        // Act
        var isValid = Validator.TryValidateObject(flower, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(-10, "Quantity must be between 0 and 1000.")]
    [InlineData(1500, "Quantity must be between 0 and 1000.")]
    public void Flower_InvalidFlowerQuantity_ProducesValidationError(int quantity, string expectedErrorMessage)
    {
        // Arrange
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            Price = 10.5m,
            Description = "A beautiful red rose.",
            FlowerQuantity = quantity,
            FlowerImageUrl = "https://example.com/rose.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(flower);

        // Act
        var isValid = Validator.TryValidateObject(flower, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Fact]
    public void Flower_InvalidUrl_ProducesValidationError()
    {
        // Arrange
        var flower = new Flower
        {
            Id = 1,
            FlowerType = "Rose",
            Price = 10.5m,
            Description = "A beautiful red rose.",
            FlowerQuantity = 50,
            FlowerImageUrl = "invalid-url",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(flower);

        // Act
        var isValid = Validator.TryValidateObject(flower, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage.Contains("The FlowerImageUrl field is not a valid fully-qualified http, https, or ftp URL."));
    }
}
