using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Fee.Services.Applying.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Applying.API.Infrastructure.Factories
{
    public class ApplyingDbContextFactory : IDesignTimeDbContextFactory<ApplyingContext>
    {
        public ApplyingContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplyingContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("Applying.API"));

            return new ApplyingContext(optionsBuilder.Options);
        }
    }
}