using Invoices.Common;
using InvoicesCRUD_BL;
using Microsoft.AspNetCore.Mvc;

namespace InvoicesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public InvoicesController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetInvoices")]
        public IEnumerable<Invoice> GetPage(QueryModel query)
        {
            return DataOperations.GetInvoicesPage(query);
        }

        [HttpGet("{id:int}", Name = "GetInvoice")]
        public Invoice? GetInvoice(int id)
        {
            return DataOperations.GetInvoice(id);
        }

        [HttpPut(Name = "CreateInvoice")]
        [Route("Create")]
        public Invoice? CreateNewInvoice(InvoiceCreationModel newItem)
        {
            return DataOperations.CreateNewInvoice(newItem);
        }
    }
}