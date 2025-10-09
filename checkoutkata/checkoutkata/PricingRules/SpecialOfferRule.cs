namespace checkoutkata.PricingRules
{
    public class SpecialOfferRule : IPricingRule
    {
        private readonly Dictionary<string, (int Quantity, int Price)> _offers;
        private readonly Dictionary<string, int> _unitPrices; // For remainders

        public SpecialOfferRule(
            Dictionary<string, (int Quantity, int Price)> offers,
            Dictionary<string, int> unitPrices)
        {
            _offers = offers ?? new Dictionary<string, (int Quantity, int Price)>();
            _unitPrices = unitPrices ?? new Dictionary<string, int>();
        }

        public int CalculatePriceFor(string sku, int count)
        {
            if (!_offers.TryGetValue(sku, out var offer)) return 0;

            int groups = count / offer.Quantity; // Number of offer groups (e.g., 3 A's)
            int remainder = count % offer.Quantity; // Remaining items after offers
            int remainderPrice = _unitPrices.TryGetValue(sku, out int unit) ? remainder * unit : 0;
            return groups * offer.Price + remainderPrice; // Offer price + remainder at unit price
        }
    }
}
