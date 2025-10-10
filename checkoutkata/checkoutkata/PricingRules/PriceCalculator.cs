namespace checkoutkata.PricingRules
{

    public class PriceCalculator : IPriceCalculator
    {

        public int CalculateTotal(Dictionary<string, int> itemCounts, IEnumerable<IPricingRule> strategies)
        {
            // Step 1: Filter items with positive counts
            var validItems = itemCounts.Where(kvp => kvp.Value > 0).ToList();
            // Example: [('A', 3), ('B', 2), ('C', 1), ('D', 1)]

            // Step 2: Calculate base price (SKU-specific or bulk deal)
            var priceRuleStrategy = strategies.OfType<PriceRuleStrategy>().FirstOrDefault();
            int baseTotal;
            if (priceRuleStrategy != null)
            {
                // Calculate SKU-specific prices
                var itemPrices = validItems.Select(kvp => new
                {
                    Sku = kvp.Key,
                    Price = priceRuleStrategy.CalculatePriceFor(kvp.Key, kvp.Value)
                }).ToList();
                int skuTotal = itemPrices.Sum(item => item.Price);

                // Choose minimum (bulk deal or SKU-specific)
                baseTotal = skuTotal;
            }
            else
            {
                baseTotal = 0;
            }
            // Example: 3 A's = min(130, 120) = 120

            // Step 3: Apply discount
            var discountStrategy = strategies.OfType<DiscountStrategy>().FirstOrDefault();
            var finalTotal = discountStrategy != null ? discountStrategy.ApplyDiscount(baseTotal) : baseTotal;
            // Example: 120 * 0.9 = 108

            return finalTotal;
        }

    }
}


