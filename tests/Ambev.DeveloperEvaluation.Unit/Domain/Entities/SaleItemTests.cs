using System;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact(DisplayName = "SaleItem: Should calculate no discount when quantity is less than 4")]
        public void Given_QuantityLessThan4_When_CalculateDiscount_Then_NoDiscountApplied()
        {
            // Arrange: Generate a sale item and override properties as needed.
            var saleItem = SaleItemSpecificationTestData.GenerateSaleItem();
            saleItem.Quantity = 3;
            saleItem.UnitPrice = 100m;
            saleItem.CalculateDiscount();

            // Act & Assert
            Assert.Equal(0m, saleItem.Discount);
            Assert.Equal(3 * 100m, saleItem.TotalAmount);
        }

        [Fact(DisplayName = "SaleItem: Should apply 10% discount when quantity is between 4 and 9")]
        public void Given_QuantityBetween4And9_When_CalculateDiscount_Then_10PercentDiscountApplied()
        {
            // Arrange
            var saleItem = SaleItemSpecificationTestData.GenerateSaleItem();
            saleItem.Quantity = 5;
            saleItem.UnitPrice = 100m;
            saleItem.CalculateDiscount();

            // Act & Assert
            Assert.Equal(0.10m, saleItem.Discount);
            Assert.Equal(5 * 100m * 0.90m, saleItem.TotalAmount);
        }

        [Fact(DisplayName = "SaleItem: Should apply 20% discount when quantity is between 10 and 20")]
        public void Given_QuantityBetween10And20_When_CalculateDiscount_Then_20PercentDiscountApplied()
        {
            // Arrange
            var saleItem = SaleItemSpecificationTestData.GenerateSaleItem();
            saleItem.Quantity = 10;
            saleItem.UnitPrice = 50m;
            saleItem.CalculateDiscount();

            // Act & Assert
            Assert.Equal(0.20m, saleItem.Discount);
            Assert.Equal(10 * 50m * 0.80m, saleItem.TotalAmount);
        }

        [Fact(DisplayName = "SaleItem: Should throw an exception when quantity exceeds 20")]
        public void Given_QuantityGreaterThan20_When_CalculateDiscount_Then_ThrowException()
        {
            // Arrange
            var saleItem = SaleItemSpecificationTestData.GenerateSaleItem();
            saleItem.Quantity = 21;
            saleItem.UnitPrice = 10m;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => saleItem.CalculateDiscount());
        }
    }
}