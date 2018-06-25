using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;

    public class ScraperContextFactory : IDesignTimeDbContextFactory<ScraperContext>
    {
        public ScraperContext CreateDbContext(string[] args)
        {
            return CreateDbContext("Server=localhost;Database=scraper;Username=postgres;Password=Server=localhost;Database=scraper;Username=postgres;Password=postgres");
        }

        public ScraperContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScraperContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ScraperContext(optionsBuilder.Options);
        }
    }
}
