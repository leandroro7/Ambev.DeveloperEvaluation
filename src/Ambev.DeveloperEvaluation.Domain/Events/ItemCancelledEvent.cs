using System;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent
    {
        public Guid SaleId { get; }
        public Guid ItemId { get; }

        public ItemCancelledEvent(Guid saleId, Guid itemId)
        {
            SaleId = saleId;
            ItemId = itemId;
        }
    }
}