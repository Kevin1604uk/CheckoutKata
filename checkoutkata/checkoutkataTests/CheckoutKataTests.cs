using checkoutkata;
using checkoutkata.PricingRules;
using Xunit;

namespace checkoutkataTests
{
    public class CheckoutKataTests
    {
        private IEnumerable<IPricingRule> _defaultRules = TestHelpers.GetDefaultStrategies();
        private IEnumerable<IPricingRule> _customRules = TestHelpers.GetWeekendStrategies();
        private IEnumerable<IPricingRule> _bankHolidayRules = TestHelpers.GetBankHolidayStrategies();

        private IEnumerable<IPricingRule> _invalidRulesNegativePrice = TestHelpers.GetNegativePriceStrategies();

        // Logging use Console for simplicity
        // or Windows Event Log (needs administrator permission)
        private ILogger _logger = new ConsoleLogger(); // new EventLogger();
        private Basket _basket;
        private PriceCalculator _calculator;

        public CheckoutKataTests()
        {
            _basket = new Basket(_logger);
            _calculator = new PriceCalculator();
        }

        #region empty basket
        // Test empty checkout
        [Fact]
        public void Basket_EmptyCheckout_TotalPriceIsZero()
        {
            // Arrange
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);

            // Act & Assert
            Assert.Equal(0, checkout.GetTotalPrice());
        }
        #endregion

        #region single and multiple items
        // test simple single item cases
        [Fact]
        public void Basket_ScanOneC_TotalPriceIs20()
        {
            // Arrange
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("C");

            // Act & Assert
            Assert.Equal(20, checkout.GetTotalPrice());
        }

        // test simple multiple item cases without offers
        [Fact]
        public void Basket_ScanTwoC_TotalPriceIs40()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("C");
            checkout.Scan("C");
            Assert.Equal(40, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_ScanThreeCDeleteC_TotalPriceIs40()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("C");
            checkout.Scan("C");
            checkout.Scan("C");
            checkout.Remove("C");
            Assert.Equal(40, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_ScanInAnyOrder_CAndD_TotalPriceIs55()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("C"); // 20
            checkout.Scan("D"); // +15
            checkout.Scan("C"); // +20
            Assert.Equal(55, checkout.GetTotalPrice()); // Wait, 20+15+20=55, fixed in comment
        }
        #endregion

        #region special offers and mixed items
        // test special offer cases
        [Fact]
        public void Basket_ScanThreeA_TotalPriceIs130()
        {
            var checkout = new CheckoutKata(_basket, _calculator, TestHelpers.GetDefaultStrategies());
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.Equal(130, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_ScanFourA_TotalPriceIs180()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.Equal(180, checkout.GetTotalPrice());
        }

        // test mixed item cases (simple + offer)
        [Fact]
        public void Basket_MixedItemsWithAOffer_TotalPriceIsCorrect()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A"); // 130
            checkout.Scan("C"); // +20
            Assert.Equal(150, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_ScanTwoB_TotalPriceIs45()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.Equal(45, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_ScanThreeB_TotalPriceIs75()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.Equal(75, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_DifferentScanSequence_TotalPriceIs95()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            checkout.Scan("B"); // Counts as part of offer
            checkout.Scan("A"); // 50
            checkout.Scan("B"); // Now 2 B's = 45, total 95
            Assert.Equal(95, checkout.GetTotalPrice());
        }

        [Fact]
        public void Basket_AllItemsWithOffers_TotalPriceIs210()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            // 3 A: 130
            // 2 B: 45
            // 1 C: 20
            // 1 D: 15
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("C");
            checkout.Scan("D");
            Assert.Equal(210, checkout.GetTotalPrice());
        }
        #endregion

        #region invalid and edge cases
        // test edge or invalid item cases
        [Fact]
        public void Basket_ScanInvalidItem_ThrowsException()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            Assert.Throws<ArgumentException>(() => checkout.Scan("XX"));
        }

        [Fact]
        public void Basket_ScanNullItem_ThrowsException()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
            Assert.Throws<ArgumentException>(() => checkout.Scan(null));
        }


        [Fact]
        public void PricingRules_PriceIsNegative_ThrowsException()
        {
            // Arrange
            var pricingRule = new PricingRule("A", -10);

            // Assert
            Assert.Equal(MessageHelpers.ErrorItemUnitPriceCannotBeNegative("A"), pricingRule.ReturnMessage);

        }



        #endregion

        #region custom pricing rules
        // test custom pricing rules
        [Fact]
        public void PricingRules_ForWeekend_NotMeetSpecialOffer_Is150()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _customRules); // DI
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            // With weekend rules, 4 A's = 160 overriden default 3 A's = 130
            Assert.Equal(150, checkout.GetTotalPrice()); // Uses custom offer
        }

        [Fact]
        public void PricingRules_ForWeekend_MeetSpecialOffer_Is160()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _customRules); // DI
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            // With weekend rules, only 4 A's = 160, not 3 A's = 130
            Assert.Equal(160, checkout.GetTotalPrice()); // Uses custom offer
        }

        [Fact]
        public void PricingRules_ForBankHoliday_NotMeetSpecialOffer_Is150()
        {

            var checkout = new CheckoutKata(_basket, _calculator, _bankHolidayRules); // DI
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            // With bank holiday rules, 4 A's = 150 overriden default 3 A's = 130
            Assert.Equal(150, checkout.GetTotalPrice()); // Uses custom offer
        }

        [Fact]
        public void PricingRules_ForBankHoliday_MeetSpecialOffer_Is150()
        {

            var checkout = new CheckoutKata(_basket, _calculator, _bankHolidayRules); // DI
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            // With bank holiday rules, 4 A's = 150 overriden default 3 A's = 130
            Assert.Equal(150, checkout.GetTotalPrice()); // Uses custom offer
        }
        #endregion

        #region test performance
        // test performance with large number of items
        [Fact]
        public void Performance_ScanLargeNumberOfItems_ReturnsExpectedTotal()
        {
            var checkout = new CheckoutKata(_basket, _calculator, _defaultRules);
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
        #endregion

        #region Discount Tests
        [Fact]
        public void PricingRules_Disccount_ScanMultipleItems_AppliedCorrectly()
        {
            var discountRules = TestHelpers.GetDiscountStrategies(TestHelpers.DefaultPriceRules, 10); // 10% discount
            var checkout = new CheckoutKata(_basket, _calculator, discountRules);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A"); // 130
            checkout.Scan("B");
            checkout.Scan("B"); // +45
            // Total before discount: 175
            // After 10% discount: 157.5 -> 157 (integer)
            Assert.Equal(157, checkout.GetTotalPrice());
        }

        #endregion
    }
}
