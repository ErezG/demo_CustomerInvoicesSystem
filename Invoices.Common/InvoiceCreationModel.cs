using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Invoices.Common
{
    public class InvoiceCreationModel
    {
        public DateTime CreatedOn = DateTime.UtcNow;
        public ProcessingStatuses Status = ProcessingStatuses.New;

        [RequiredDoubleFieldAttribute(ErrorMessage = "Amount Must not be zero.")]
        public double Amount { get; set; }

        [RequiredEnumFieldAttribute(ErrorMessage = "Method value is invalid.")]
        public PaymentMethods Method { get; set; }
    }
}
