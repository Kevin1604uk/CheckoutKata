namespace checkoutkata.PricingRules
{
    public interface IPriceCalculator
    {
        int CalculateTotal(Dictionary<string, int> itemCounts, IEnumerable<IPricingRule> pricingRules);
    }
}
