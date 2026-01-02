using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationServices.models
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public Guid TicketId { get; set; }
        public string InvoiceHTMLDocumentPath { get; set; } = string.Empty;
        public string TicketSerial { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NumberofHours { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public decimal TotalAmountWithOutTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}