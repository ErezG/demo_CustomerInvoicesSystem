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
            var invoicesContext = new InvoicesContext(contextOptions);
            builder.Services.AddSingleton<InvoicesContext>(invoicesContext);
            //builder.Services.AddDbContext<InvoicesContext>(options =>
            //    options.UseSqlite(dbConnectionString,
            //                      contextOptionsBuilder => contextOptionsBuilder.MigrationsAssembly("InvoicesAPI")));
            InvoicesContext.SetInstance(invoicesContext);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}