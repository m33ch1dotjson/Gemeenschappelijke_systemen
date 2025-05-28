using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class RompslompInvoiceDto
    {
        [JsonPropertyName("sales_invoice")]
        public SalesInvoice SalesInvoice { get; set; } = new();
    }

    public class SalesInvoice
    {
        public string Date { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "pay_transfer";
        public string? Description { get; set; }
        public long ContactId { get; set; }
        public string Currency { get; set; } = "eur";
        public string CurrencyExchangeRate { get; set; } = "1.0";
        public string? VatNumber { get; set; }
        public string ApiReference { get; set; } = string.Empty;
        public string PaymentReference { get; set; } = string.Empty;
        public string SaleType { get; set; } = "";
        public bool DistanceSale { get; set; } = false;
        public List<InvoiceLine> InvoiceLines { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
    }

    public class InvoiceLine
    {
        public string Description { get; set; } = string.Empty;
        public string? ExtendedDescription { get; set; }
        public string PricePerUnit { get; set; } = "0.0";
        public string VatRate { get; set; } = "0.21";
        public long? VatTypeId { get; set; }
        public string Quantity { get; set; } = "1.0";
        public long? ProductId { get; set; }
        public long? AccountId { get; set; }
        public string? AccountPath { get; set; }
    }

    public class Payment
    {
        public string Amount { get; set; } = "0.0";
        public string? Description { get; set; }
        public long AccountId { get; set; }
        public string AccountPath { get; set; } = string.Empty;
        public string PaidAt { get; set; } = string.Empty;
    }

}
