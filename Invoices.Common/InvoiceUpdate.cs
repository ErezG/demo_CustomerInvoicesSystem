namespace Invoices.Common
{
    public class InvoiceUpdate
    {
        public DateTime UpdatedOn = DateTime.UtcNow;

        public int InvoiceId { get; set; }

        [OptionalEnumFieldAttribute(ErrorMessage = "Status value is invalid.")]
        public ProcessingStatuses? Status { get; set; }

        [OptionalDoubleFieldAttribute(ErrorMessage = "Amount Must not be zero.")]
        public double? Amount { get; set; }

        [OptionalEnumFieldAttribute(ErrorMessage = "Method value is invalid.")]
        public PaymentMethods? Method { get; set; }
    }
}
