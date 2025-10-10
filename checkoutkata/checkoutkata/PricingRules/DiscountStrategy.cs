namespace checkoutkata.PricingRules
{

    /// <summary>
    /// Applies a percentage discount to the total price.
    /// SRP: Only handles discount application.
    /// </summary>
    public class DiscountStrategy : IPricingRule
    {
        private readonly decimal _discountPercentage;

        public DiscountStrategy(decimal discountPercentage)
        {
            _discountPercentage = discountPercentage;
        }

        public int CalculatePriceFor(string sku, int count)
        {
            return 0; // Not used for individual SKUs
        }

        // New: Apply discount to total
        public int ApplyDiscount(int total)
        {
            return (int)((1 - _discountPercentage) * total);
        }
    }
}
