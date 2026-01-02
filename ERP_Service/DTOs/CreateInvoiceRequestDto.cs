using Microsoft.AspNetCore.Mvc;

namespace IntegrationServices.DTOs
{
    public class CreateInvoiceRequestDto
    {
       
        public string InvoiceHTMLDocument { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid TicketId { get; set; }
        public string TicketSerial { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public decimal TotalAmountWithOutTax { get; set; }
        public decimal TaxAmount { get; set; }
    }
}