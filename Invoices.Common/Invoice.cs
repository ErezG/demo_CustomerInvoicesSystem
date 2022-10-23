namespace Invoices.Common
{
    public class Invoice
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int InvoiceId { get; set; }
        public ProcessingStatuses Status { get; set; }
        public double Amount { get; set; }

        public PaymentMethods Method { get; set; }
    }

    public enum ProcessingStatuses
    {
        New = 1,
        Paid,
        Canceled
    }

    public enum PaymentMethods
    {
        CreditCard = 1,
        DebitCard,
        ElectronicCheck
    }
}
