# CheckoutKata
Checkout Kata task

Checkout Kata app in .NET 8. Scan items, gets total, and handle special offers
using TDD implementation

Pricing Rules
A -> 50 each or 3 for 130
B -> 30 each or 2 for 45
C -> 20 each
D -> 15 each

How?
Checkout = scan items and get the total
ICheckout = the contract
DefaultPricingRules = hardcoded prices (A, B, C, D)
CustomPricingRules = lets you plug in your own prices/offers

Things it handles:
Bulk deals (like 3 for 130/ 2 for 45)
Order doesn’t matter when scanning
Easy to swap in new pricing rules
Ignores invalid input instead of crashing
Full xUnit test coverage
Running tests