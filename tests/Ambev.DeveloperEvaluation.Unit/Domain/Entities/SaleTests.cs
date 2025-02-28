using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale: Should calculate total based on sale items")]
        public void Given_SaleWithMultipleItems_When_CalculateTotal_Then_TotalAmountIsSumOfItemTotals()
        {
            // Arrange
            var sale = new Sale
            {
                SaleNumber = "S100",
                SaleDate = DateTime.UtcNow,
                Customer = "Customer Test",
                Branch = "BranchX",
                Items = new List<SaleItem>
                {
                    // SaleItem with quantity 5, UnitPrice 100 → discount 10% → total = 450
                    new SaleItem { Quantity = 5, UnitPrice = 100 },
                    // SaleItem with quantity 3, UnitPrice 200 → discount 0% → total = 600
                    new SaleItem { Quantity = 3, UnitPrice = 200 }
                }
            };

            // Act
            sale.CalculateTotal();

            // Assert
            Assert.Equal(450m + 600m, sale.TotalAmount);
        }
    }
}