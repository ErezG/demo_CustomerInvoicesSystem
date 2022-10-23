﻿using Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL_EF_SQLite
{
    public static class EFOperations
    {
        public static List<Invoice> GetPage(IEnumerable<Expression<Func<Invoice, bool>>>? filters, IEnumerable<QuerySortModel>? sortPriorities, int pageNum, int pageSize)
        {
            var invoicesTable = InvoicesContext.Instance.Invoices;

            IQueryable<Invoice> filteredInvoices = invoicesTable;
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    filteredInvoices = filteredInvoices.Where(filter);
                }
            }

            var orderedInvoices = filteredInvoices;
            if (sortPriorities != null)
            {
                bool isFirstSort = true;
                foreach (var sort in sortPriorities)
                {
                    orderedInvoices = orderedInvoices.OrderingHelper(sort.Field, sort.Ascending, isFirstSort);
                    isFirstSort = false;
                }
            }

            var retrievedItems = orderedInvoices.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return retrievedItems;
        }

        public static Invoice? GetInvoice(int id)
        {
            var invoicesTable = InvoicesContext.Instance.Invoices;

            Invoice? invoice = invoicesTable.FirstOrDefault(item => item.InvoiceId == id);
            return invoice;
        }

        public static Invoice? AddNewInvoice(Invoice newItem)
        {
            var invoicesTable = InvoicesContext.Instance.Invoices;

            var createdEntity = invoicesTable.Add(newItem);
            InvoicesContext.Instance.SaveChanges();
            return createdEntity.Entity.InvoiceId > 0 ? createdEntity.Entity : null;
        }

        public static Invoice? UpdateInvoice(Invoice existingItem)
        {
            var invoicesTable = InvoicesContext.Instance.Invoices;

            var createdEntity = invoicesTable.Update(existingItem);
            var changes = InvoicesContext.Instance.SaveChanges();
            return changes > 0 ? createdEntity.Entity : null;
        }
    }
}
