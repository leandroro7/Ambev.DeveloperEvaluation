using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent
    {
        public Guid SaleId { get; }
        public SaleCreatedEvent(Guid saleId) => SaleId = saleId;
    }
}