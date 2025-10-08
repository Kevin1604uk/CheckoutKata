using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkoutkata
{
    public static class TestHelpers
    {
        public static IEnumerable<PricingRule> GetDefaultRules()
        {
            return new PricingRule[]
            {
                new("A", 50, 3, 130),
                new("B", 30, 2, 45),
                new("C", 20),
                new("D", 15)
            };
        }

        public static IEnumerable<PricingRule> GetCustomRulesForWeekend()
        {
            return new List<PricingRule>
            {
                new("A", 50, 3, 100), // Cheaper offer for test
                new("C", 20)
            };
        }

        public static IEnumerable<PricingRule> GetCustomRulesForBankHoliday()
        {
            return new List<PricingRule>
            {
                new("A", 50, 3, 100), // Cheaper offer for test
                new("B", 30, 2, 40), // Cheaper offer for test
                new("C", 20)
            };
        }
    }
}
