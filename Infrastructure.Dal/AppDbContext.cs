using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal
{

    public class AppDbContext : DbContext
    {

        private static string ConnectionStringName = "App";

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }

}