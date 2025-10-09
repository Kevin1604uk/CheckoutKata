using checkoutkata.PricingRules;

namespace checkoutkata
{
    public class CheckoutKata : ICheckout
    {
        private readonly IBasket _basket;
        private readonly IPriceCalculator _calculator;
        private readonly IEnumerable<IPricingRule> _pricingRules;

        public CheckoutKata(
            IBasket basket,
            IPriceCalculator calculator,
            IEnumerable<IPricingRule> pricingRules)
        {
            _basket = basket ?? throw new ArgumentNullException(nameof(basket));
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _pricingRules = pricingRules?.ToList() ?? Enumerable.Empty<IPricingRule>();
        }

        public void Scan(string item) => _basket.AddItem(item);

        public void Remove(string item) => _basket.RemoveItem(item); // Delegate to Basket


        public int GetTotalPrice()
        {
            try
            {
                var counts = _basket.GetItemCounts();
                return _calculator.CalculateTotal(counts, _pricingRules);
            }
            catch (Exception ex)
            {
                // Graceful: Log via injected logger (assume available); return 0 or partial total
                // In full impl, inject ILogger here too
                Console.Error.WriteLine(String.Format(MessageHelpers.ErrorCalculation, ex.Message));
                return 0;
            }
        }

    }
}
