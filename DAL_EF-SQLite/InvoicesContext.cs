using Invoices.Common;
using Microsoft.EntityFrameworkCore;

namespace DAL_EF_SQLite
{
    public class InvoicesContext : DbContext
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();

        public InvoicesContext(DbContextOptions<InvoicesContext> options)
            : base(options) { }

        public static InvoicesContext? Instance { get; private set; }
        public static void SetInstance(InvoicesContext instance) => Instance = instance;
    }
}
