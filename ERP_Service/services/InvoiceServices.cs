using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegrationServices.DataBase;
using IntegrationServices.DTOs;
using IntegrationServices.models;
using Microsoft.EntityFrameworkCore;

namespace IntegrationServices.services
{
    public class InvoiceServices
    {
        private readonly AppDbContext _context;

        public InvoiceServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoice(CreateInvoiceRequestDto request)
        {
            if (await IsTicketIdExistsAsync(request.TicketId))
            {
                throw new InvalidOperationException("An invoice with this TicketId already exists.");
            }

            int numberofHours = CalculateNumberOfHours(request.FromDate, request.ToDate);
            decimal totalAmount = CalculateTotalAmount(request.TotalAmountWithOutTax, request.TaxAmount);
            Invoice newInvoice = new Invoice
            {
                InvoiceId = request.InvoiceId,
                TicketId = request.TicketId,
                TicketSerial = request.TicketSerial,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                NumberofHours = numberofHours,
                PlateNumber = request.PlateNumber,
                TotalAmountWithOutTax = request.TotalAmountWithOutTax,
                TaxAmount = request.TaxAmount,
                TotalAmount = totalAmount
            };
            var uploader = new CloudinaryUploader();
            newInvoice.InvoiceHTMLDocumentPath = await uploader.UploadHtmlAsync(request.InvoiceHTMLDocument, request.InvoiceId.ToString());

            _context.Invoices.Add(newInvoice);
            await _context.SaveChangesAsync();

            return newInvoice;
        }
        public async Task<List<InvoiceResponseDTO>> GetAllInovices()
        {
            var inovices = _context.Invoices.ToList();
            var inovicesDto = new List<InvoiceResponseDTO>();
            foreach (var invoice in inovices)
            {
                var invoiceDto = new InvoiceResponseDTO
                {
                    InvoiceHTMLDocumentPath = invoice.InvoiceHTMLDocumentPath,
                    TicketSerial = invoice.TicketSerial,
                    FromDate = invoice.FromDate,
                    ToDate = invoice.ToDate,
                    NumberofHours = invoice.NumberofHours,
                    PlateNumber = invoice.PlateNumber,
                    TotalAmountWithOutTax = invoice.TotalAmountWithOutTax,
                    TaxAmount = invoice.TaxAmount,
                    TotalAmount = invoice.TotalAmount,
                };
                inovicesDto.Add(invoiceDto);
            }
            return inovicesDto;
        }

        private async Task<bool> IsTicketIdExistsAsync(Guid ticketId)
        {
            return await _context.Invoices.AnyAsync(i => i.TicketId == ticketId);
        }
        private int CalculateNumberOfHours(DateTime fromDate, DateTime toDate)
        {
            TimeSpan duration = toDate - fromDate;
            return (int)duration.TotalHours;
        }
        private decimal CalculateTotalAmount(decimal totalAmountWithOutTax, decimal taxAmount)
        {
            return totalAmountWithOutTax + taxAmount;
        }
    }
}