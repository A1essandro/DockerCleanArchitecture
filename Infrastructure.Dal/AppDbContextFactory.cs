using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Dal
{

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options, null, null);
        }
    }

}