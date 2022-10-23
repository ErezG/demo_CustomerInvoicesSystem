namespace Invoices.Common
{
    public class InvoiceCreationModel
    {
        public DateTime CreatedOn = DateTime.UtcNow;
        public ProcessingStatuses Status = ProcessingStatuses.New;
        public double Amount { get; set; }
        public PaymentMethods Method { get; set; }
    }
}
