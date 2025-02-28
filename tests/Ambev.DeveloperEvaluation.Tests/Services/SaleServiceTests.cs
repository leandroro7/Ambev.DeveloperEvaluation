using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Infrastructure.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;

namespace Ambev.DeveloperEvaluation.Tests.Services
{
    public class SaleServiceTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SaleService> _logger;
        private readonly ISaleService _saleService;

        public SaleServiceTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _logger = Substitute.For<ILogger<SaleService>>();
            _saleService = new SaleService(_saleRepository, _eventPublisher, _logger);
        }

        [Fact]
        public async Task CreateSaleAsync_Should_CalculateTotal_And_Publish_SaleCreatedEvent()
        {
            // Arrange: use test data for generating a valid sale.
            var sale = SaleSpecificationTestData.GenerateSale();
            
            // Act
            var result = await _saleService.CreateSaleAsync(sale);

            // Assert: Validate that the total is the sum of sale items.
            var expectedTotal = sale.Items.Sum(item => item.TotalAmount);
            Assert.Equal(expectedTotal, result.TotalAmount);
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>());
            await _eventPublisher.Received(1)
                .PublishAsync(Arg.Is<SaleCreatedEvent>(e => e.SaleId == result.Id));
        }

        [Fact]
        public async Task UpdateSaleAsync_Should_Update_And_Publish_SaleModifiedEvent()
        {
            // Arrange: Set up an existing sale using the test data generator.
            var saleId = Guid.NewGuid();
            var existingSale = SaleSpecificationTestData.GenerateSale();
            existingSale.Id = saleId;

            // Generate updated sale data using the test helper and override some properties.
            var updatedSale = SaleSpecificationTestData.GenerateSale();
            updatedSale.SaleNumber = "S002 Updated";
            updatedSale.Customer = "UpdatedCustomer";
            updatedSale.Branch = "UpdatedBranch";
            
            _saleRepository.GetByIdAsync(saleId).Returns(existingSale);

            // Act
            var result = await _saleService.UpdateSaleAsync(saleId, updatedSale);

            // Assert
            Assert.Equal("S002 Updated", result.SaleNumber);
            Assert.Equal("UpdatedCustomer", result.Customer);
            Assert.Equal("UpdatedBranch", result.Branch);
            var expectedTotal = result.Items.Sum(item => item.TotalAmount);
            Assert.Equal(expectedTotal, result.TotalAmount);
            await _saleRepository.Received(1).UpdateAsync(existingSale);
            await _eventPublisher.Received(1)
                .PublishAsync(Arg.Is<SaleModifiedEvent>(e => e.SaleId == saleId));
        }

        [Fact]
        public async Task GetSaleByIdAsync_Should_Throw_Exception_When_NotFound()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            _saleRepository.GetByIdAsync(saleId).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _saleService.GetSaleByIdAsync(saleId));
        }

        [Fact]
        public async Task CancelSaleAsync_Should_Mark_Sale_And_Items_As_Cancelled_And_Publish_Events()
        {
            // Arrange: Generate a sale with test data.
            var saleId = Guid.NewGuid();
            var sale = SaleSpecificationTestData.GenerateSale();
            sale.Id = saleId;
            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            
            // Act
            await _saleService.CancelSaleAsync(saleId);

            // Assert: Ensure sale and all sale items are marked as cancelled.
            Assert.True(sale.Cancelled);
            foreach (var item in sale.Items)
            {
                Assert.True(item.Cancelled);
            }
            await _saleRepository.Received(1).UpdateAsync(sale);
            await _eventPublisher.Received(1)
                .PublishAsync(Arg.Is<SaleCancelledEvent>(e => e.SaleId == saleId));
            await _eventPublisher.Received(sale.Items.Count)
                .PublishAsync(Arg.Any<ItemCancelledEvent>());
        }
    }
}