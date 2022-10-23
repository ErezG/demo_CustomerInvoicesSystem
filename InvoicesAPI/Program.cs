using DAL_EF_SQLite;
using Microsoft.EntityFrameworkCore;

namespace InvoicesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var dbConnectionString = builder.Configuration.GetValue<string>("DB:SQLite:ConnectionString");
            var contextOptions = new DbContextOptionsBuilder<InvoicesContext>()
                .UseSqlite(dbConnectionString,
                           contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("InvoicesAPI"))
                .Options;
            builder.Services.AddSingleton<InvoicesContext>(new InvoicesContext(contextOptions));
            //builder.Services.AddDbContext<InvoicesContext>(options =>
            //    options.UseSqlite(dbConnectionString,
            //                      contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("InvoicesAPI")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            Test(app).Wait();
            
            app.Run();
        }

        public static async Task Test(WebApplication app)
        {
            var dbContext = app.Services.GetService<InvoicesContext>();
            if (dbContext.Invoices.Any() == false)
            {
                await CreateInitialData(dbContext);
            }
            dbContext.Invoices.ToList().ForEach(entry => Console.WriteLine(entry));
        }

        public static async Task CreateInitialData(InvoicesContext dbContext)
        {
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-03-11 08:11:20"), InvoiceId = 0001, Status = (ProcessingStatuses)3, Amount = 10.00     , Method = (PaymentMethods)1 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-03-12 08:04:12"), InvoiceId = 0002, Status = (ProcessingStatuses)2, Amount = 120.00    , Method = (PaymentMethods)3 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-03-11 22:01:39"), InvoiceId = 0003, Status = (ProcessingStatuses)3, Amount = 30.00     , Method = (PaymentMethods)2 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-04-08 12:37:08"), InvoiceId = 0004, Status = (ProcessingStatuses)1, Amount = 1.00      , Method = (PaymentMethods)3 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-03-16 10:41:39"), InvoiceId = 0005, Status = (ProcessingStatuses)1, Amount = 10.12     , Method = (PaymentMethods)3 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-05-14 10:59:17"), InvoiceId = 0006, Status = (ProcessingStatuses)2, Amount = 5.00      , Method = (PaymentMethods)1 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-04-09 18:20:32"), InvoiceId = 0007, Status = (ProcessingStatuses)3, Amount = 1230.00   , Method = (PaymentMethods)1 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-03-16 11:13:38"), InvoiceId = 0008, Status = (ProcessingStatuses)2, Amount = 132210.00 , Method = (PaymentMethods)2 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-04-13 11:37:09"), InvoiceId = 0009, Status = (ProcessingStatuses)3, Amount = 10.00     , Method = (PaymentMethods)1 });
            dbContext.Invoices.Add(new Invoice() { CreatedOn = DateTime.Parse("2020-05-14 10:53:19"), InvoiceId = 0010, Status = (ProcessingStatuses)1, Amount = 133.00    , Method = (PaymentMethods)3 });
            await dbContext.SaveChangesAsync();
        }
    }
}