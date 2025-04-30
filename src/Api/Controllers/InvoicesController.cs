using System.Globalization;
using Application.Invoices;
using Application.Invoices.CreateInvoice;
using Application.Invoices.ExportInvoice;
using Application.Invoices.GetExportLogById;
using Application.Invoices.GetExportLogsList;
using Application.Invoices.GetInvoiceById;
using Application.Invoices.GetInvoicesList;
using Application.Invoices.GetOverdueInvoicePaymentsAnalysis;
using Application.Invoices.TotalNumberOfInvoices.GetInvoiceCount;
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

        [HttpGet("{id:guid}/export-pdf")]
        public async Task<IActionResult> GetExportPdfAsync([FromRoute] Guid clientId, [FromRoute] Guid id)
        {
            var result = await Mediator.Send(new ExportInvoiceCommand(id, clientId));

            return File(result.ByteArray, result.ContentType, result.FileName);
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetAllLogsAsync([FromRoute] Guid clientId)
        {
            var result = await Mediator.Send(new GetExportLogsListQuery(clientId));

            return Ok(result);
        }

        [HttpGet("logs/{logId:guid}")]
        public async Task<IActionResult> GetLogByIdAsync([FromRoute] Guid clientId, [FromRoute] Guid logId)
        {
            var result = await Mediator.Send(new GetExportLogByIdQuery(clientId, logId));

            return Ok(result);
        }

        [HttpGet("overdue-payments-analysis")]
        public async Task<IActionResult> GetOverduePaymentsAnalysisAsync([FromRoute] Guid clientId)
        {
            var result = await Mediator.Send(new GetOverdueInvoicePaymentsAnalysisQuery(clientId));

            return Ok(result);
        }

        [HttpGet("total-number-of-invoices-By-DateRange")]
        public async Task<IActionResult> GetTotalNumberOfInvoicesWithinDateRange(
            [FromRoute] Guid clientId,
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            [FromQuery] Guid? customerId = null)
        {
            if (!DateTime.TryParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedStartDate))
            {
                return BadRequest(new { Message = "Invalid format for startDate. Expected format: dd.MM.yyyy (e.g., 25.04.2025)." });
            }

            if (!DateTime.TryParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedEndDate))
            {
                return BadRequest(new { Message = "Invalid format for endDate. Expected format: dd.MM.yyyy (e.g., 25.04.2025)." });
            }

            parsedStartDate = DateTime.SpecifyKind(parsedStartDate, DateTimeKind.Utc);
            parsedEndDate = DateTime.SpecifyKind(parsedEndDate, DateTimeKind.Utc);

            var result = await Mediator.Send(new GetInvoiceCountQuery(clientId)
            {
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                CustomerId = customerId,
            });

            return Ok(result);
        }
    }
}
