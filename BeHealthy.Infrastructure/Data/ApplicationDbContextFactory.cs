using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BeHealthy.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var configuration = new ConfigurationBuilder()
               .AddUserSecrets<ApplicationDbContextFactory>()
               .Build();

            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseMySQL(connectionString!);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
