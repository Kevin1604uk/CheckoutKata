namespace checkoutkata.PricingRules
{
    public class PriceRuleStrategy : IPricingRule
    {
        private List<PricingRule> _pricingRules;

        // DI: Inject list of PriceRules
        public PriceRuleStrategy(List<PricingRule> priceRules)
        {
            _pricingRules = priceRules ?? new List<PricingRule>();
        }

        public int CalculatePriceFor(string sku, int count)
        {
            var rule = _pricingRules.FirstOrDefault(r => r.Sku == sku);
            if (rule == null) return 0; // Unknown SKU

            // If no special offer or count too low, use unit price
            if (!rule.SpecialQuantity.HasValue || !rule.SpecialPrice.HasValue || count < rule.SpecialQuantity.Value)
            {
                return count * rule.UnitPrice;
            }

            // Apply special offer: groups + remainder
            int groups = count / rule.SpecialQuantity.Value;
            int remainder = count % rule.SpecialQuantity.Value;
            return groups * rule.SpecialPrice.Value + remainder * rule.UnitPrice;
        }
    }
}
