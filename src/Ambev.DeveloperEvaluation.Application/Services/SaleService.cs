using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SaleService> _logger;

        public SaleService(ISaleRepository saleRepository, IEventPublisher eventPublisher, ILogger<SaleService> logger)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                throw new Exception("Sale not found");
            return sale;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            // Enforce business rules for each sale item
            foreach (var item in sale.Items)
            {
                item.CalculateDiscount();
            }
            sale.CalculateTotal();

            await _saleRepository.CreateAsync(sale);
            await _eventPublisher.PublishAsync(new SaleCreatedEvent(sale.Id));
            _logger.LogInformation("Sale created with Id: {SaleId}", sale.Id);
            return sale;
        }

        public async Task<Sale> UpdateSaleAsync(Guid id, Sale sale)
        {
            var existingSale = await _saleRepository.GetByIdAsync(id);
            if (existingSale == null)
                throw new Exception("Sale not found");
            
            // Update properties following the UserService update model
            existingSale.SaleNumber = sale.SaleNumber;
            existingSale.SaleDate = sale.SaleDate;
            existingSale.Customer = sale.Customer;
            existingSale.Branch = sale.Branch;
            existingSale.Items = sale.Items;

            foreach (var item in existingSale.Items)
            {
                item.CalculateDiscount();
            }
            existingSale.CalculateTotal();

            await _saleRepository.UpdateAsync(existingSale);
            await _eventPublisher.PublishAsync(new SaleModifiedEvent(existingSale.Id));
            _logger.LogInformation("Sale modified with Id: {SaleId}", existingSale.Id);
            return existingSale;
        }

        public async Task CancelSaleAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                throw new Exception("Sale not found");

            sale.Cancelled = true;
            await _saleRepository.UpdateAsync(sale);
            await _eventPublisher.PublishAsync(new SaleCancelledEvent(sale.Id));
            _logger.LogInformation("Sale cancelled with Id: {SaleId}", sale.Id);

            // Cancel each item that is not canceled
            foreach (var item in sale.Items)
            {
                if (!item.Cancelled)
                {
                    item.Cancelled = true;
                    await _eventPublisher.PublishAsync(new ItemCancelledEvent(sale.Id, item.Id));
                    _logger.LogInformation("Sale item cancelled with Id: {ItemId} in sale {SaleId}", item.Id, sale.Id);
                }
            }
        }
    }
}