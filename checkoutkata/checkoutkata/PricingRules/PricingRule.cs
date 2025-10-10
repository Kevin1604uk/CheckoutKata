namespace checkoutkata.PricingRules
{
    public class PricingRule
    {
        public string Sku { get; }
        public int UnitPrice { get; }
        public int? SpecialQuantity { get; }
        public int? SpecialPrice { get; }

        public string ReturnMessage { get; set; } = string.Empty;

        public PricingRule(string sku, int unitPrice, int? specialQuantity = null, int? specialPrice = null)
        {
            Sku = sku;
            UnitPrice = unitPrice;
            SpecialQuantity = specialQuantity;
            SpecialPrice = specialPrice;

            ReturnMessage = Validate(sku, unitPrice, specialQuantity, specialPrice);

        }

        private string Validate(string sku, int unitPrice, int? specialQuantity, int? specialPrice)
        {
            if (unitPrice < 0)
            {
                return MessageHelpers.ErrorItemUnitPriceCannotBeNegative(sku);
            }
            if (specialQuantity.HasValue && specialQuantity <= 0)
            {
                return MessageHelpers.ErrorItemSpecialQuantityMustBeGreaterThanZero(sku);
            }
            if (specialPrice.HasValue && specialPrice < 0)
            {
                return MessageHelpers.ErrorItemSpecialPriceCannotBeNegative(sku);
            }
            if (specialQuantity.HasValue && specialPrice.HasValue && specialPrice >= unitPrice * specialQuantity)
            {
                return MessageHelpers.ErrorItemSpecialPriceMustBeLessThanTotalUnitPrice(sku);
            }

            return string.Empty; // No errors

        }
    }
}
