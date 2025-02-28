using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DefaultContext _context;

        public SaleItemRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale item in the database.
        /// </summary>
        public async Task<SaleItem> CreateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
        {
            await _context.SaleItems.AddAsync(saleItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return saleItem;
        }

        /// <summary>
        /// Retrieves a sale item by its unique identifier.
        /// </summary>
        public async Task<SaleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.SaleItems.FirstOrDefaultAsync(si => si.Id == id, cancellationToken);
        }
        
        /// <summary>
        /// Retrieves all sale items.
        /// </summary>
        public async Task<IEnumerable<SaleItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaleItems.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an existing sale item.
        /// </summary>
        public async Task UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
        {
            _context.SaleItems.Update(saleItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a sale item from the database.
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var saleItem = await GetByIdAsync(id, cancellationToken);
            if (saleItem == null)
                return false;

            _context.SaleItems.Remove(saleItem);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}