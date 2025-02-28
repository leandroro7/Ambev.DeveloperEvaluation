using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData
{
    /// <summary>
    /// Provides methods for generating test data for Sale entities using the Bogus library.
    /// This class centralizes all test data generation for Sale tests to ensure consistency across test cases.
    /// </summary>
    public static class SaleSpecificationTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid SaleItem entities.
        /// The generated SaleItems will have appropriate quantity, unit price, and computed total.
        /// The discount is set to 0 by default because it is calculated via business logic on execution.
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
        /// Configures the Faker to generate valid Sale entities.
        /// The generated Sale will have a sale number, sale date, customer (denormalized),
        /// branch, and a collection of SaleItem entities.
        /// </summary>
        private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale
            {
                SaleNumber = f.Commerce.Ean13(),
                SaleDate = f.Date.Past(),
                Customer = f.Company.CompanyName(),
                Branch = f.Address.City(),
                Cancelled = false,
                Items = saleItemFaker.Generate(f.Random.Number(1, 3))
            });

        /// <summary>
        /// Generates a valid Sale entity with random data.
        /// </summary>
        /// <returns>A valid Sale entity populated with test data.</returns>
        public static Sale GenerateSale()
        {
            var sale = saleFaker.Generate();
            // Business logic usually calculates discounts later; ensure totals are recalculated if needed.
            foreach (var item in sale.Items)
            {
                item.CalculateDiscount();
            }
            sale.CalculateTotal();
            return sale;
        }
    }
}