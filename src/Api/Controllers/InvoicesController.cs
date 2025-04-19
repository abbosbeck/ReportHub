using Application.Invoices;
using Application.Invoices.CreateInvoice;
using Application.Invoices.GetInvoiceById;
using Application.Invoices.GetInvoicesList;
using Application.Invoices.UpdateInvoice;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("clients/{clientId:guid}/[controller]")]
    public class InvoicesController(ISender mediator) : ApiControllerBase(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreateInvoiceRequest request)
        {
            var result = await Mediator.Send(new CreateInvoiceCommand(clientId, request));

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid clientId, [FromBody] UpdateInvoiceRequest request)
        {
            var result = await Mediator.Send(new UpdateInvoiceCommand(clientId, request));

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetInvoiceByIdQuery { Id = id, ClientId = clientId });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromRoute] Guid clientId)
        {
            var result = await Mediator.Send(new GetInvoicesListQuery(clientId));

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
        {
            var result = await Mediator.Send(new DeleteInvoiceCommand { Id = id, ClientId = clientId });

            return Ok(result);
        }
    }
}
