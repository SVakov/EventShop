using EventShopApp.Enums;
using EventShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class OrderDetailModelTests
{
    [Fact]
    public void OrderDetail_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var orderDetail = new OrderDetail
        {
            Id = 1,
            OrderId = 1,
            Order = new Order { Id = 1, DateOfOrder = DateTime.UtcNow, DeadLineDate = DateTime.UtcNow.AddDays(5), Status = OrderStatus.Pending },
            FlowerId = 1,
            Flower = new Flower { Id = 1, FlowerType = "Rose", Price = 10.0m, FlowerQuantity = 100 },
            OrderedFlowerQuantity = 5,
            Type = OrderType.Flower,
            ArrangementItemsId = null,
            ArrangementItem = null,
            OrderedArrangementQuantity = null,
            IsPrepayed = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderDetail);

        // Act
        var isValid = Validator.TryValidateObject(orderDetail, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(0, "The OrderId field is required and must be greater than 0.")]
    [InlineData(-1, "The OrderId field is required and must be greater than 0.")]
    public void OrderDetail_InvalidOrderId_ProducesValidationError(int orderId, string expectedErrorMessage)
    {
        // Arrange
        var orderDetail = new OrderDetail
        {
            Id = 1,
            OrderId = orderId,
            Type = OrderType.Flower,
            IsPrepayed = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderDetail);

        // Act
        var isValid = Validator.TryValidateObject(orderDetail, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage.Contains(expectedErrorMessage));
    }

    [Fact]
    public void OrderDetail_InvalidType_ProducesValidationError()
    {
        // Arrange
        var orderDetail = new OrderDetail
        {
            Id = 1,
            OrderId = 1,
            Type = (OrderType)999,
            IsPrepayed = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderDetail);

        // Act
        var isValid = Validator.TryValidateObject(orderDetail, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage.Contains("The value provided for Type is invalid."));
    }

    [Fact]
    public void OrderDetail_MissingFlowerAndArrangementItem_ProducesNoValidationErrors()
    {
        // Arrange
        var orderDetail = new OrderDetail
        {
            Id = 1,
            OrderId = 1,
            Type = OrderType.Flower,
            FlowerId = null,
            ArrangementItemsId = null,
            IsPrepayed = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderDetail);

        // Act
        var isValid = Validator.TryValidateObject(orderDetail, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void OrderDetail_NullFlowerAndArrangementItem_DoesNotProduceValidationErrors()
    {
        // Arrange
        var orderDetail = new OrderDetail
        {
            Id = 1,
            OrderId = 1,
            Type = OrderType.Arrangement,
            FlowerId = null,
            ArrangementItemsId = null,
            IsPrepayed = true
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(orderDetail);

        // Act
        var isValid = Validator.TryValidateObject(orderDetail, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }
}
