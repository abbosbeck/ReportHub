using Application.Invoices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class InvoiceController(ISender mediator) : ApiControllerBase(mediator)
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var result = await Mediator.Send(new GetInvoiceByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var result = await Mediator.Send(new GetAllInvoicesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("by-client/{clientId}")]
        public async Task<IActionResult> GetInvoicesByClientId(Guid clientId)
        {
            var result = await Mediator.Send(new GetInvoicesByClientIdQuery { clientId = clientId });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteInvoice(Guid id)
        {
            var result = await Mediator.Send(new DeleteInvoiceCommand { Id = id });
            return Ok(result);
        }
    }
}
