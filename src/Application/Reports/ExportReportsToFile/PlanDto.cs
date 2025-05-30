﻿namespace Application.Reports.ExportReportsToFile;

public class PlanDto
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public decimal TotalPrice { get; set; }

    public string CurrencyCode { get; set; }
}
