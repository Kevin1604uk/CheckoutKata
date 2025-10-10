namespace checkoutkata
{
    public static class MessageHelpers
    {
        public const string WelcomeMessage = "Welcome to the Checkout Kata!";

        public static string Error(string message) => $"Error: {message}";
        public static string ErrorItemMustBeASingleCharacter() => $"Item must be a single character SKU.";
        public static string ErrorCalculatingTotalPrice() => $"Error calculating total price.";
        public static string ErrorInvalidItemScanned(string sku) => $"Invalid item scanned: item {sku}";
        public static string ErrorCalculation(string calculation) => $"Calculation error: {calculation}";

        public static string ErrorItemUnitPriceCannotBeNegative(string sku) => $"Error: Unit price for SKU '{sku}' cannot be negative.";
        public static string ErrorItemSpecialQuantityMustBeGreaterThanZero(string sku) => $"Error: Special quantity for SKU '{sku}' must be greater than zero.";
        public static string ErrorItemSpecialPriceCannotBeNegative(string sku) => $"Error: Special price for SKU '{sku}' cannot be negative.";
        public static string ErrorItemSpecialPriceMustBeLessThanTotalUnitPrice(string sku) => $"Error: Special price for SKU '{sku}' must be less than the total unit price for the special quantity.";

        public static string InfoItemAdded(string sku) => $"Item '{sku}' added to basket.";

    }
}
