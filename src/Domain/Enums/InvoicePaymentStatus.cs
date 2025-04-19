namespace Domain.Enums;

public enum InvoicePaymentStatus
{
    /// <summary>
    /// Fully paid payment
    /// </summary>
    Paid,

    /// <summary>
    /// Partially paid payment
    /// </summary>
    PartiallyPaid,

    /// <summary>
    /// Unpaid payment
    /// </summary>
    Unpaid,
}
