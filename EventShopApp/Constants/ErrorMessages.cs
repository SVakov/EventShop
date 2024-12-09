namespace EventShopApp.Constants
{
    public static class ErrorMessages
    {
        public const string MaxArrangementItemTypeLength = "Arrangement Type cannot be longer than 50 characters.";
        public const string PriceMustBePositive = "Price must be higher than zero.";
        public const string MaxDescriptionLength = "Description cannot be longer than 200 characters.";
        public const string ArrangementQuantityRange = "Quantity must be between 0 and 500.";
        public const string MaxNameLength = "Name cannot be longer than 50 characters.";
        public const string MaxSurnameLength = "Surname cannot be longer than 50 characters.";
        public const string MaxPhoneNumberLength = "Phone number cannot be longer than 15 characters.";
        public const string MaxEmailLength = "Email cannot be longer than 50 characters.";
        public const string InvalidEmailFormat = "Invalid email address format.";
        public const string MaxAddressLength = "Address cannot be longer than 50 characters.";
        public const string MaxFlowerTypeLength = "Flower Type cannot be longer than 20 characters.";
        public const string FlowerQuantityRange = "Quantity must be between 0 and 1000.";
        public const string ClientIdRequired = "The ClientId field is required and must be greater than 0.";
        public const string OrderIdRequired = "The OrderId field is required and must be greater than 0.";
        public const string InvalidOrderType = "The value provided for Type is invalid.";
    }
}
