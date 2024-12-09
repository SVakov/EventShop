using EventShopApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class ClientModelTests
{
    [Fact]
    public void Client_ValidModel_DoesNotProduceValidationErrors()
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            Address = "123 Main St"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "The Name field is required.")]
    [InlineData("", "The Name field is required.")]
    [InlineData("ThisNameIsWayTooLongAndExceedsFiftyCharactersWhichIsInvalid", "Name cannot be longer than 50 characters.")]
    public void Client_InvalidName_ProducesValidationError(string name, string expectedErrorMessage)
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = name,
            Surname = "Doe",
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            Address = "123 Main St"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData("ThisSurnameIsWayTooLongAndExceedsFiftyCharactersWhichIsInvalid", "Surname cannot be longer than 50 characters.")]
    public void Client_InvalidSurname_ProducesValidationError(string surname, string expectedErrorMessage)
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = "John",
            Surname = surname,
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            Address = "123 Main St"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "The PhoneNumber field is required.")]
    [InlineData("", "The PhoneNumber field is required.")]
    [InlineData("1234567890123456", "Phone number cannot be longer than 15 characters.")]
    public void Client_InvalidPhoneNumber_ProducesValidationError(string phoneNumber, string expectedErrorMessage)
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            PhoneNumber = phoneNumber,
            Email = "john.doe@example.com",
            Address = "123 Main St"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "The Email field is required.")]
    [InlineData("", "The Email field is required.")]
    [InlineData("invalid-email", "Invalid email address format.")]
    [InlineData("ThisEmailIsWayTooLongAndExceedsFiftyCharacters@example.com", "Email cannot be longer than 50 characters.")]
    public void Client_InvalidEmail_ProducesValidationError(string email, string expectedErrorMessage)
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "123456789",
            Email = email,
            Address = "123 Main St"
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData("ThisAddressIsWayTooLongAndExceedsFiftyCharactersWhichIsInvalid", "Address cannot be longer than 50 characters.")]
    public void Client_InvalidAddress_ProducesValidationError(string address, string expectedErrorMessage)
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "123456789",
            Email = "john.doe@example.com",
            Address = address
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(client);

        // Act
        var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage == expectedErrorMessage);
    }
}
