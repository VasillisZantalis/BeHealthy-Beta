using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BeHealthy.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            //var connectionString = "Host=behealthydb;Port=5432;Database=behealthy;Username=admin;Password=7530";
            var connectionString = "Host=dpg-cus7h6rv2p9s73ataba0-a;Port=5432;Database=behealthy_w1ur;Username=behealthy_w1ur_user;Password=MAAzUNXPSpDiGW7weNC3voseF8FL8zq2";

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Default' not found.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
