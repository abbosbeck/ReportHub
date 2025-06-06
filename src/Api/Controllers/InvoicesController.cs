﻿using System.ComponentModel.DataAnnotations;
using Application.Invoices;
using Application.Invoices.CreateInvoice;
using Application.Invoices.ExportInvoice;
using Application.Invoices.GetExportLogById;
using Application.Invoices.GetExportLogsList;
using Application.Invoices.GetInvoiceById;
using Application.Invoices.GetInvoicesList;
using Application.Invoices.GetOverdueInvoicePaymentsAnalysis;
using Application.Invoices.GetTotalNumberOfInvoices;
using Application.Invoices.GetTotalRevenueCalculation;
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
            var result = await Mediator.Send(new GetInvoiceByIdQuery(clientId, id));

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
            var result = await Mediator.Send(new DeleteInvoiceCommand(clientId, id));

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

        [HttpGet("total-revenue-calculation")]
        public async Task<IActionResult> TotalRevenueCalucationAsync(
            [FromRoute] Guid clientId,
            [FromQuery, Required] DateTime startDate,
            [FromQuery, Required] DateTime endDate)
        {
            var result = await Mediator.Send(new GetTotalRevenueCalculationQuery(clientId, startDate, endDate));

            return Ok(result);
        }

        [HttpGet("total-number-of-invoices-by-daterange")]
        public async Task<IActionResult> GetTotalNumberOfInvoicesWithinDateRange(
              [FromRoute] Guid clientId,
              [FromQuery] DateTime startDate,
              [FromQuery] DateTime endDate,
              [FromQuery] Guid? customerId = null)
        {
            var result = await Mediator.Send(new GetInvoiceCountQuery(clientId, startDate, endDate, customerId));

            return Ok(result);
        }
    }
}