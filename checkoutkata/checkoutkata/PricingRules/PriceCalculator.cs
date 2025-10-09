namespace checkoutkata.PricingRules
{

    public class PriceCalculator : IPriceCalculator
    {

        public int CalculateTotal(Dictionary<string, int> itemCounts, IEnumerable<IPricingRule> strategies)
        {
            // Step 1: Filter items with positive counts
            var validItems = itemCounts.Where(kvp => kvp.Value > 0).ToList();
            // Example: [('A', 3), ('B', 2), ('C', 1), ('D', 1)]

            // Step 2: Calculate price for each item (single strategy)
            var itemPrices = validItems.Select(kvp => new
            {
                Sku = kvp.Key,
                Price = strategies.First().CalculatePriceFor(kvp.Key, kvp.Value) // Only one strategy
            }).ToList();
            // Example: [{ 'A', 130 }, { 'B', 45 }, { 'C', 20 }, { 'D', 15 }]

            // Step 3: Sum the prices
            var total = itemPrices.Sum(item => item.Price);
            // Example: 130 + 45 + 20 + 15 = 210

            return total;
        }
        /*
        private int CalculateMinPrice(string sku, int count, IEnumerable<IPricingRule> pricingRules)
        {
            // Step 2.1: Get prices from all strategies
            var allPrices = pricingRules.Select(s => s.CalculatePriceFor(sku, count)).ToList();
            // Intermediate result (for A, count=3): allPrices = [150, 130] (UnitPrice: 3*50, SpecialOffer: 1*130)

            // Step 2.2: Filter positive prices
            var positivePrices = allPrices.Where(price => price > 0).ToList();
            // Intermediate result (for A): positivePrices = [150, 130]

            // Step 2.3: Default to 0 if empty, then take minimum
            var minPrice = positivePrices.Any() ? positivePrices.Min() : 0;
            // Intermediate result (for A): minPrice = 130

            return minPrice;
        }
        */
    }
}


