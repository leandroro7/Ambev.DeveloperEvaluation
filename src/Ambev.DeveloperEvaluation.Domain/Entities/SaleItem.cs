using System;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public string Product { get; set; } // External identity (denormalized)
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } // Percentage discount (0, 0.10, or 0.20)
        public decimal TotalAmount { get; set; }
        public bool Cancelled { get; set; }
        
        public void CalculateDiscount()
        {
            if (Quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");
            
            if (Quantity < 4)
                Discount = 0;
            else if (Quantity < 10)
                Discount = 0.10m;
            else // Quantity between 10 and 20 inclusive
                Discount = 0.20m;
                
            TotalAmount = (Quantity * UnitPrice) * (1 - Discount);
        }
    }
}