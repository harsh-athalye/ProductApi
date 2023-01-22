using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductApi.Models;

namespace ProductApi
{
    public class ProductContext : DbContext
    {
        protected readonly IConfiguration configuration;

        public ProductContext(IConfiguration config)
        {
            configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
        }

        public DbSet<Product> Products { get; set; }
    }
}
