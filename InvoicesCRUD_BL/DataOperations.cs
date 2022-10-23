using DAL_EF_SQLite;
using Invoices.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesCRUD_BL
{
    public static class DataOperations
    {
        public static IEnumerable<Invoice> GetInvoicesPage(QueryModel query)
        {
            var filters = query.Filters?.Select(filterText => filterText.ProcessFilter<Invoice>()).ToArray();
            var invoicesPage = EFOperations.GetPage(filters, query.Sort, query.Paging.PageNum, query.Paging.PageSize);
            return invoicesPage;
        }

        public static Invoice? GetInvoice(int invoiceId)
        {
            var invoice = EFOperations.GetInvoice(invoiceId);
            return invoice;
        }

        public static Invoice? CreateNewInvoice(InvoiceCreationModel data)
        {
            var newInvoice = new Invoice()
            {
                CreatedOn = data.CreatedOn,
                Status = data.Status,
                Amount = data.Amount,
                Method = data.Method
            };
            var invoice = EFOperations.AddNewInvoice(newInvoice);
            return invoice;
        }

        public static Invoice? UpdateInvoice(InvoiceUpdate changes)
        {
            var existingInvoice = GetInvoice(changes.InvoiceId);
            if (existingInvoice == null)
            {
                throw new KeyNotFoundException();
            }

            if (!changes.Status.HasValue || changes.Status == existingInvoice.Status &&
                !changes.Amount.HasValue || changes.Amount == existingInvoice.Amount &&
                !changes.Method.HasValue || changes.Method == existingInvoice.Method)
            {
                return null;
            }
            
            existingInvoice.UpdatedOn = changes.UpdatedOn;
            existingInvoice.Status = changes.Status ?? existingInvoice.Status;
            existingInvoice.Amount = changes.Amount ?? existingInvoice.Amount;
            existingInvoice.Method = changes.Method ?? existingInvoice.Method;

            var invoice = EFOperations.UpdateInvoice(existingInvoice);
            return invoice;
        }
    }
}
