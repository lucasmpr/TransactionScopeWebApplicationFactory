using System;
using System.Linq;
using System.Threading;
using DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Test
{
    public class ApiFactory: WebApplicationFactory<Startup>
    {
        bool disposed;

        public string dbName; 
        public string conn;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            dbName = Guid.NewGuid().ToString();
            conn =  $"Server=.\\SQLEXPRESS;Database={dbName};Integrated Security=True;";
            
            builder.ConfigureServices(ConfigureServices);

            builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole(options => options.IncludeScopes = true);
            });

        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            
            
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<BloggingContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<BloggingContext>(options =>
            {
                options.UseSqlServer(conn);
                options.EnableSensitiveDataLogging();
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope(); // Sweet C# 8.0
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<BloggingContext>();
            
            db.Database.EnsureCreated();
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    using var scope = Server.Services.CreateScope();
                    var db = scope.ServiceProvider.GetService<BloggingContext>();
                    db.Database.EnsureDeleted();
                }
                base.Dispose(disposing);
            }
            //dispose unmanaged resources
            disposed = true;
        }
    }
}
