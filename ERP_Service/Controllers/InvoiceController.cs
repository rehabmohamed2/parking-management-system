using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegrationServices.DTOs;
using IntegrationServices.services;
namespace IntegrationServices.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly InvoiceServices _invoiceServices;

        public InvoiceController(ILogger<InvoiceController> logger, InvoiceServices invoiceServices)
        {
            _logger = logger;
            _invoiceServices = invoiceServices;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequestDto request)
        {
            Console.WriteLine("invoice recieved: " + request.PlateNumber);
            try
            {
                var newInvoice = await _invoiceServices.CreateInvoice(request);
                return Ok(new { Message = "Invoice created successfully", InvoiceId = newInvoice.InvoiceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("all")]
        public async Task<List<InvoiceResponseDTO>> GetAll()
        {
            return await _invoiceServices.GetAllInovices();
        }

    }
}