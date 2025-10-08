namespace checkoutkata
{
    public class CheckoutKata : ICheckout
    {

        private readonly List<PricingRule> _rules;
        private readonly Dictionary<string, int> _itemCounts = new();

        public CheckoutKata(IEnumerable<PricingRule> rules)
        {
            _rules = rules?.ToList() ?? new List<PricingRule>();
        }

        /// <summary>
        /// Scans an item (single-character SKU).
        /// </summary>
        /// <param name="item">The SKU (e.g., "A").</param>
        /// <exception cref="ArgumentException">If item is invalid.</exception>
        public void Scan(string item)
        {
            if (string.IsNullOrEmpty(item) || item.Length != 1)
                throw new ArgumentException("Item must be a single character SKU.", nameof(item));

            string sku = item;
            if (!_itemCounts.ContainsKey(sku))
                _itemCounts[sku] = 0;
            _itemCounts[sku]++;
        }

        /// <summary>
        /// Calculates the total price, applying special offers where applicable.
        /// Order of scanning does not matter; counts are used for pricing.
        /// </summary>
        public int GetTotalPrice()
        {
            int total = 0;
            foreach (var kvp in _itemCounts)
            {
                string sku = kvp.Key;
                int count = kvp.Value;
                var rule = _rules.FirstOrDefault(r => r.Sku == sku);
                if (rule == null) continue; // Ignore unknown SKUs

                total += CalculatePriceForItem(rule, count);
            }
            return total;
        }

        private static int CalculatePriceForItem(PricingRule rule, int count)
        {
            if (!rule.SpecialQuantity.HasValue)
                return count * rule.UnitPrice;

            int groups = count / rule.SpecialQuantity.Value;
            int remainder = count % rule.SpecialQuantity.Value;
            return (groups * rule.SpecialPrice!.Value) + (remainder * rule.UnitPrice);
        }

    }
}
