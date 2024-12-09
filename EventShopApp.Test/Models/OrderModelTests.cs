using EventShopApp.Enums;
using EventShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class OrderModelTests
{
    [Fact]
    public void Order_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            ClientId = 1,
            Client = new Client { Id = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "123456789" },
            DateOfOrder = DateTime.UtcNow,
            DeadLineDate = DateTime.UtcNow.AddDays(5),
            Status = OrderStatus.Pending,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail
                {
                    Id = 1,
                    OrderId = 1,
                    FlowerId = 1,
                    OrderedFlowerQuantity = 5,
                    Type = OrderType.Flower
                }
            }
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(order);

        // Act
        var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(0, "The ClientId field is required and must be greater than 0.")]
    [InlineData(-1, "The ClientId field is required and must be greater than 0.")]
    public void Order_InvalidClientId_ProducesValidationError(int clientId, string expectedErrorMessage)
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            ClientId = clientId,
            DateOfOrder = DateTime.UtcNow,
            DeadLineDate = DateTime.UtcNow.AddDays(5),
            Status = OrderStatus.Pending
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(order);

        // Act
        var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage.Contains(expectedErrorMessage));
    }


    [Theory]
    [InlineData("2022-01-01", "2020-01-01")]
    public void Order_InvalidDates_ShouldNotThrowValidationErrorInModel(string dateOfOrderStr, string deadlineDateStr)
    {
        // Arrange
        var dateOfOrder = DateTime.Parse(dateOfOrderStr);
        var deadlineDate = DateTime.Parse(deadlineDateStr);

        var order = new Order
        {
            Id = 1,
            ClientId = 1,
            DateOfOrder = dateOfOrder,
            DeadLineDate = deadlineDate,
            Status = OrderStatus.Pending
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(order);

        // Act
        var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }


    [Fact]
    public void Order_NullOrderDetails_DoesNotProduceValidationErrors()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            ClientId = 1,
            DateOfOrder = DateTime.UtcNow,
            DeadLineDate = DateTime.UtcNow.AddDays(5),
            Status = OrderStatus.Pending,
            OrderDetails = null
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(order);

        // Act
        var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Order_EmptyOrderDetails_DoesNotProduceValidationErrors()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            ClientId = 1,
            DateOfOrder = DateTime.UtcNow,
            DeadLineDate = DateTime.UtcNow.AddDays(5),
            Status = OrderStatus.Pending,
            OrderDetails = new List<OrderDetail>()
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(order);

        // Act
        var isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }
}
