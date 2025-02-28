using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } // External identity (denormalized)
        public decimal TotalAmount { get; set; }
        public string Branch { get; set; }
        public bool Cancelled { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        public void CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                item.CalculateDiscount();
                total += item.TotalAmount;
            }
            TotalAmount = total;
        }
    }
}