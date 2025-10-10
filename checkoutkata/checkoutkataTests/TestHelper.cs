using checkoutkata.PricingRules;

namespace checkoutkata
{
    public static class TestHelpers
    {
        public static List<PricingRule> DefaultPriceRules { get; } = new List<PricingRule>
        {
            new PricingRule("A", 50, 3, 130), // A: 50 each, 3 for 130
            new PricingRule("B", 30, 2, 45), // B: 30 each, 2 for 45
            new PricingRule("C", 20), // C: 20 each
            new PricingRule("D", 15) // D: 15 each
        };

        public static IEnumerable<IPricingRule> GetDefaultStrategies()
        {
            return new IPricingRule[]
            {
                new PriceRuleStrategy(DefaultPriceRules) // OCP: Single strategy handles all pricing
            };
        }

        public static IEnumerable<IPricingRule> GetWeekendStrategies()
        {
            return new IPricingRule[]
            {
            new PriceRuleStrategy(WeekendPriceRules) // No pricing rules
            };
        }

        public static IEnumerable<IPricingRule> GetBankHolidayStrategies()
        {
            return new IPricingRule[]
            {
            new PriceRuleStrategy(BankHolidayPriceRules) // No pricing rules
            };
        }

        public static IEnumerable<IPricingRule> GetNegativePriceStrategies()
        {
            return new IPricingRule[]
            {
            new PriceRuleStrategy(NegativePriceRules) // No pricing rules
            };
        }

        public static List<PricingRule> EmptyPriceRules { get; } = new List<PricingRule>();

        public static List<PricingRule> NegativePriceRules { get; } = new List<PricingRule>
        {
            new PricingRule("A", -50) // Cheaper offer
        };
        // Test-specific: Custom PriceRules for testing overrides
        public static List<PricingRule> WeekendPriceRules { get; } = new List<PricingRule>
        {
            new PricingRule("A", 50, 4, 160) // Cheaper offer
        };

        public static List<PricingRule> BankHolidayPriceRules { get; } = new List<PricingRule>
        {
            new PricingRule("A", 50, 4, 150) // Cheaper offer
        };


        public static IEnumerable<IPricingRule> GetDiscountStrategies(List<PricingRule> pricingRules, int discount)
        {
            return new IPricingRule[]
            {
                new PriceRuleStrategy(pricingRules),
                new DiscountStrategy(decimal.Parse(discount.ToString()) / 100) // 10% discount
            };
        }




    }
}
