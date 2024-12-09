using EventShopApp.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class ArrangementItemModelTests
{
    [Fact]
    public void ArrangementItem_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var arrangement = new ArrangementItem
        {
            Id = 1,
            ArrangementItemType = "Bouquet",
            Price = 25.0m,
            Description = "A beautiful flower bouquet.",
            ArrangementItemsQuantity = 100,
            ArrangementItemImageUrl = "https://example.com/bouquet.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(arrangement);

        // Act
        var isValid = Validator.TryValidateObject(arrangement, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "The ArrangementItemType field is required.")]
    [InlineData("ThisArrangementTypeNameIsWayTooLongForValidation", "Arrangement Type cannot be longer than 50 characters.")]
    public void ArrangementItem_InvalidArrangementType_ProducesValidationError(string arrangementType, string expectedErrorMessage)
    {
        // Arrange
        var arrangement = new ArrangementItem
        {
            Id = 1,
            ArrangementItemType = arrangementType,
            Price = 25.0m,
            Description = "A beautiful flower bouquet.",
            ArrangementItemsQuantity = 100,
            ArrangementItemImageUrl = "https://example.com/bouquet.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(arrangement);

        // Act
        var isValid = Validator.TryValidateObject(arrangement, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(-1, "Price must be higher than zero.")]
    [InlineData(0, "Price must be higher than zero.")]
    public void ArrangementItem_InvalidPrice_ProducesValidationError(decimal price, string expectedErrorMessage)
    {
        // Arrange
        var arrangement = new ArrangementItem
        {
            Id = 1,
            ArrangementItemType = "Bouquet",
            Price = price,
            Description = "A beautiful flower bouquet.",
            ArrangementItemsQuantity = 100,
            ArrangementItemImageUrl = "https://example.com/bouquet.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(arrangement);

        // Act
        var isValid = Validator.TryValidateObject(arrangement, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(-10, "Quantity must be between 0 and 500.")]
    [InlineData(600, "Quantity must be between 0 and 500.")]
    public void ArrangementItem_InvalidQuantity_ProducesValidationError(int quantity, string expectedErrorMessage)
    {
        // Arrange
        var arrangement = new ArrangementItem
        {
            Id = 1,
            ArrangementItemType = "Bouquet",
            Price = 25.0m,
            Description = "A beautiful flower bouquet.",
            ArrangementItemsQuantity = quantity,
            ArrangementItemImageUrl = "https://example.com/bouquet.jpg",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(arrangement);

        // Act
        var isValid = Validator.TryValidateObject(arrangement, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Fact]
    public void ArrangementItem_InvalidUrl_ProducesValidationError()
    {
        // Arrange
        var arrangement = new ArrangementItem
        {
            Id = 1,
            ArrangementItemType = "Bouquet",
            Price = 25.0m,
            Description = "A beautiful flower bouquet.",
            ArrangementItemsQuantity = 100,
            ArrangementItemImageUrl = "invalid-url",
            IsAvailable = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(arrangement);

        // Act
        var isValid = Validator.TryValidateObject(arrangement, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage.Contains("The ArrangementItemImageUrl field is not a valid fully-qualified http, https, or ftp URL."));
    }
}
