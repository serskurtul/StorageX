using System;

namespace StorageX.Models
{
    public enum InvoiceType
    {
        Sell,
        Buy
    }
    public class Invoice
    {
        public Guid Id { get; set; }
        public InvoiceType Type { get; set; }
        public int ItemId { get; set; }
        public double Amount { get; set; }
        public decimal Total { get; set; }
        public DateTime DateTime { get; set; }

        public Invoice()
        {
            DateTime = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
