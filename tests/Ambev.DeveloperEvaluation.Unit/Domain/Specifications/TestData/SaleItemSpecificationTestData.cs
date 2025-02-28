using System;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData
{
    /// <summary>
    /// Provides methods for generating test data for SaleItem entities using the Bogus library.
    /// This class centralizes all test data generation for SaleItem tests to ensure consistency across test cases.
    /// </summary>
    public static class SaleItemSpecificationTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid SaleItem entities.
        /// The generated SaleItem will have a quantity between 1 and 20,
        /// a unit price between 10 and 500, with discount set to 0 by default.
        /// The TotalAmount is computed via business logic (using CalculateDiscount).
        /// </summary>
        private static readonly Faker<SaleItem> saleItemFaker = new Faker<SaleItem>()
            .CustomInstantiator(f => new SaleItem
            {
                Quantity = f.Random.Number(1, 20),
                UnitPrice = f.Finance.Amount(10, 500),
                Discount = 0, // default discount; will be recalculated
                TotalAmount = 0, // will be calculated with CalculateDiscount
                Cancelled = false
            });

        /// <summary>
        /// Generates a valid SaleItem entity with random test data.
        /// Calls CalculateDiscount to ensure business logic is applied.
        /// </summary>
        /// <returns>A valid SaleItem entity populated with test data.</returns>
        public static SaleItem GenerateSaleItem()
        {
            var saleItem = saleItemFaker.Generate();
            // Apply business logic to compute discount and total amount.
            saleItem.CalculateDiscount();
            return saleItem;
        }
    }
}