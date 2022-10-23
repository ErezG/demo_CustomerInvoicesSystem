using Microsoft.EntityFrameworkCore;

namespace DAL_EF_SQLite
{
    public class InvoicesContext : DbContext
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();

        public InvoicesContext(DbContextOptions<InvoicesContext> options)
            : base(options) { }
    }
}
