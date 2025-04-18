using Application.Invoices;
using Application.Invoices.GetInvoice;
using Application.Invoices.UpdateInvoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("clients/{clientId:guid}/[controller]")]
    public class InvoicesController(ISender mediator) : ApiControllerBase(mediator)
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid clientId, [FromBody] CreateInvoiceRequest request)
        {
            var result = await Mediator.Send(new CreateInvoiceCommand(clientId, request));

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateInvoiceCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new GetInvoiceCommand { Id = id });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await Mediator.Send(new DeleteInvoiceCommand { Id = id });

            return Ok(result);
        }
    }
}
