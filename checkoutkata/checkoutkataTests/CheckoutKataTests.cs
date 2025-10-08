using Xunit;
using checkoutkata;
using System.Runtime.CompilerServices;

namespace checkoutkataTests
{
    public class CheckoutKataTests
    {
        private IEnumerable<PricingRule> defaultRules = TestHelpers.GetDefaultRules();
        private IEnumerable<PricingRule> customRules = TestHelpers.GetCustomRulesForWeekend();
        private IEnumerable<PricingRule> bankHolidayRules = TestHelpers.GetCustomRulesForBankHoliday();

        private ILogger _logger = new EventLogger();

        // Test empty checkout
        [Fact]
        public void EmptyCheckout_TotalPrice_IsZero()
        {
            // Arrange
            var checkout = new CheckoutKata(defaultRules, _logger);

            // Act & Assert
            Assert.Equal(0, checkout.GetTotalPrice());
        }

        // test simple item cases
        [Fact]
        public void ScanOneC_TotalPrice_Is20()
        {
            // Arrange
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("C");

            // Act & Assert
            Assert.Equal(20, checkout.GetTotalPrice());
        }

        [Fact]
        public void ScanTwoC_TotalPrice_Is40()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("C");
            checkout.Scan("C");
            Assert.Equal(40, checkout.GetTotalPrice());
        }

        [Fact]
        public void ScanInAnyOrder_CAndD_TotalPrice_Is35()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("C"); // 20
            checkout.Scan("D"); // +15
            checkout.Scan("C"); // +20
            Assert.Equal(55, checkout.GetTotalPrice()); // Wait, 20+15+20=55, fixed in comment
        }

        // test special offer cases
        [Fact]
        public void ScanThreeA_TotalPrice_Is130()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.Equal(130, checkout.GetTotalPrice());
        }

        [Fact]
        public void ScanFourA_TotalPrice_Is180()
        {
            var checkout = new CheckoutKata (defaultRules, _logger);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.Equal(180, checkout.GetTotalPrice());
        }

        // test mixed item cases (simple + offer)
        [Fact]
        public void MixedItemsWithAOffer_TotalPrice_IsCorrect()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("A"); checkout.Scan("A"); checkout.Scan("A"); // 130
            checkout.Scan("C"); // +20
            Assert.Equal(150, checkout.GetTotalPrice());
        }

        [Fact]
        public void ScanTwoB_TotalPrice_Is45()
        {
            var checkout = new CheckoutKata (defaultRules, _logger);
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.Equal(45, checkout.GetTotalPrice());
        }

        [Fact]
        public void ScanThreeB_TotalPrice_Is75()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.Equal(75, checkout.GetTotalPrice());
        }

        [Fact]
        public void FullExample_BABSequence_TotalPrice_Is95()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            checkout.Scan("B"); // Counts as part of offer
            checkout.Scan("A"); // 50
            checkout.Scan("B"); // Now 2 B's = 45, total 95
            Assert.Equal(95, checkout.GetTotalPrice());
        }

        // test edge or invalid item cases
        [Fact]
        public void ScanInvalidItem_ThrowsException()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            Assert.Throws<ArgumentException>(() => checkout.Scan("XX"));
        }

        [Fact]
        public void AllItemsWithOffers_TotalPrice_Is210()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            // 3 A: 130
            // 2 B: 45
            // 1 C: 20
            // 1 D: 15
            checkout.Scan("A"); checkout.Scan("A"); checkout.Scan("A");
            checkout.Scan("B"); checkout.Scan("B");
            checkout.Scan("C");
            checkout.Scan("D");
            Assert.Equal(210, checkout.GetTotalPrice());
        }

        // test custom pricing rules
        [Fact]
        public void CustomRulesForWeekend_OverrideDefaultBehavior()
        {
            
            var checkout = new CheckoutKata(customRules, _logger); // DI
            checkout.Scan("A"); 
            checkout.Scan("A"); 
            checkout.Scan("A");
            Assert.Equal(100, checkout.GetTotalPrice()); // Uses custom offer
        }

        [Fact]
        public void CustomRulesForBankHoliday_OverrideDefaultBehavior()
        {

            var checkout = new CheckoutKata(bankHolidayRules, _logger); // DI
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.Equal(100, checkout.GetTotalPrice()); // Uses custom offer
        }

        // test performance with large number of items
        [Fact]
        public void ScanLargeNumberOfItems_PerformanceTest()
        {
            var checkout = new CheckoutKata(defaultRules, _logger);
            for (int i = 0; i < 1000; i++)
            {
                checkout.Scan("A");
                checkout.Scan("B");
                checkout.Scan("C");
                checkout.Scan("D");
            }
            // 1000 A's: (333 * 130) + (1 * 50) = 43350 // should be 43340
            // 1000 B's: (500 * 45) = 22500
            // 1000 C's: (1000 * 20) = 20000
            // 1000 D's: (1000 * 15) = 15000
            // Total = 43350 + 22500 + 20000 + 15000 = 100850
            Assert.Equal(100840, checkout.GetTotalPrice());
        }
    }
}
