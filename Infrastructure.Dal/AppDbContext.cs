using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Dal
{

    public class AppDbContext : DbContext
    {

        private static string ConnectionStringName = "App";

        private readonly ILogger<AppDbContext> _logger;
        private readonly IConfiguration _config;

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }

}