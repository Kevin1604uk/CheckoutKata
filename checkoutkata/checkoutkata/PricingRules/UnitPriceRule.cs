namespace checkoutkata.PricingRules
{
    public class UnitPriceRule : IPricingRule
    {
        private readonly Dictionary<string, int> _unitPrices;

        public UnitPriceRule(Dictionary<string, int> unitPrices)
        {
            _unitPrices = unitPrices ?? new Dictionary<string, int>();
        }

        public int CalculatePriceFor(string sku, int count) =>
            _unitPrices.TryGetValue(sku, out int price) ? count * price : 0;
    }
}
