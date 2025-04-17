using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        public UpdateInvoiceCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.InvoiceNumber).NotEmpty();
            RuleFor(x => x.IssueDate).NotEmpty();
            RuleFor(x => x.DueDate).NotEmpty().GreaterThan(x => x.IssueDate);
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.PaymentStatus).IsInEnum();
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Name).NotEmpty();
                item.RuleFor(i => i.Price).GreaterThan(0);
                item.RuleFor(i => i.CurrencyCode).NotEmpty();
            });
        }
    }
}
