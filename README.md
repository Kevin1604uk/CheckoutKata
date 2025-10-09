# CheckoutKata
Checkout Kata task

Checkout Kata app uses C#, .NET 8. implementing 
- scan items, gets total
- handle special offers
- follow SOLID, Dependency Injection (DI), and Rule-based for extensibility
- logging
- code coverage


Pricing Rules :
A -> 50 each or 3 for 130
B -> 30 each or 2 for 45
C -> 20 each
D -> 15 each

Classes explanation :
Main project - checkoutkata
- Checkout, ICheckout = Main checkout Kata implementation
- Basket, IBasket = Manage scanned items
- PriceCalculator, PriceCalculator = calculates totals by rules
- PricingRule, IPricingRule, UnitPriceRule, SpecialOfferRule = pricing rules (unit prices and offers)
- CheckoutTests = Tests covering all scenarios (empty, single items, offers, errors)
Test project - checkoutkataTests
- TestHelpers = Test-specific constants (empty offers, custom pricing)
  - DefaultPricingRules = hardcoded prices (A, B, C, D)
  - CustomWeedendPricingRule, CustomBankHolidayPricingRule = lets you plug in your own prices/offers
Repository management
- .gitignore = Ignores build artifacts, IDE files, etc.

Things it handles:
- Basket for items
- Bulk deals (like 3 for 130/ 2 for 45)
- Custom deals on Weekend (like 4 for 160) and Bank Holiday (4 for 150)
- Order doesn’t matter when scanning
- Flexsible to apply new pricing rules
- Error Handling to prompt understandable messages instead of crashing
- xUnit test coverage

Test Coverage :
- Empty checkout
- Single item scanning
- Multiple items scanning without special offers
- Multiple items scanning with special offers
- Mixed item combinations
- Custom pricing rules for weekend (4 for 160) and bank holiday (4 for 150)
- Error handling (null, empty, unknown items)
- Edge cases (quantities just above/below special price thresholds)

Setup Instructions :
1 clone using Git to local repository
2 Open in Visual Studio 2022
3 Open solution - checkoutkata.sln
4 Ensure all projects target .Net 8
5 If necessary, restore NuGet Packages
   - Latest System.Diagnostic.EventLog
   - Latest Microsoft.NET.Test.Sdk, xunit, xunit.runner.visualstudio

Running tests :
1 In Visual Studio 2022, open TestExploer (Test > Test Explorer)
2 Run all tests (Test > Run All Tests)