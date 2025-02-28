using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using System.Collections.Concurrent;


namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class InMemorySaleRepository : ISaleRepository
    {
        private readonly ConcurrentDictionary<Guid, Sale> _sales = new ConcurrentDictionary<Guid, Sale>();

        public Task CreateAsync(Sale sale)
        {
            sale.Id = Guid.NewGuid();
            _sales[sale.Id] = sale;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _sales.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Sale>> GetAllAsync()
        {
            return Task.FromResult(_sales.Values.AsEnumerable());
        }

        public Task<Sale> GetByIdAsync(Guid id)
        {
            _sales.TryGetValue(id, out var sale);
            return Task.FromResult(sale);
        }

        public Task UpdateAsync(Sale sale)
        {
            _sales[sale.Id] = sale;
            return Task.CompletedTask;
        }
    }
}