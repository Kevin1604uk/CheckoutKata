namespace checkoutkata.PricingRules
{
    public interface IPricingRule
    {
        int CalculatePriceFor(string sku, int count);
    }
}
