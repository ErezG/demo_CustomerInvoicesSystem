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
    }
}
