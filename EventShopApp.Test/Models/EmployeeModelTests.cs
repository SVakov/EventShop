using EventShopApp.Enums;
using EventShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class EmployeeModelTests
{
    [Fact]
    public void Employee_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            Role = EmployeeRole.Manager,
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            HireDate = DateTime.UtcNow,
            IsFired = false
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(employee);

        // Act
        var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "The Name field is required.")]
    [InlineData("", "The Name field is required.")]
    [InlineData("ThisNameIsWayTooLongAndExceedsFiftyCharactersWhichIsInvalid", "Name cannot be longer than 50 characters.")]
    public void Employee_InvalidName_ProducesValidationError(string name, string expectedErrorMessage)
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            Name = name,
            Surname = "Doe",
            Role = EmployeeRole.Manager,
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            HireDate = DateTime.UtcNow,
            IsFired = false
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(employee);

        // Act
        var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData("ThisSurnameIsWayTooLongAndExceedsFiftyCharactersWhichIsInvalid", "Surname cannot be longer than 50 characters.")]
    public void Employee_InvalidSurname_ProducesValidationError(string surname, string expectedErrorMessage)
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            Name = "John",
            Surname = surname,
            Role = EmployeeRole.Manager,
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            HireDate = DateTime.UtcNow,
            IsFired = false
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(employee);

        // Act
        var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "The PhoneNumber field is required.")]
    [InlineData("", "The PhoneNumber field is required.")]
    [InlineData("1234567890123456", "Phone number cannot be longer than 15 characters.")]
    public void Employee_InvalidPhoneNumber_ProducesValidationError(string phoneNumber, string expectedErrorMessage)
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            Role = EmployeeRole.Manager,
            PhoneNumber = phoneNumber,
            Email = "john.doe@example.com",
            HireDate = DateTime.UtcNow,
            IsFired = false
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(employee);

        // Act
        var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "The Email field is required.")]
    [InlineData("", "The Email field is required.")]
    [InlineData("invalid-email", "Invalid email address format.")]
    [InlineData("ThisEmailIsWayTooLongAndExceedsFiftyCharacters@example.com", "Email cannot be longer than 50 characters.")]
    public void Employee_InvalidEmail_ProducesValidationError(string email, string expectedErrorMessage)
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            Role = EmployeeRole.Manager,
            PhoneNumber = "123456789",
            Email = email,
            HireDate = DateTime.UtcNow,
            IsFired = false
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(employee);

        // Act
        var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }
}
