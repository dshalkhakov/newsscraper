using System;

namespace newsscraper.DAL
{
    using Microsoft.EntityFrameworkCore;

    public class ScraperContext : DbContext, IScraperContext
    {
        public ScraperContext(DbContextOptions<ScraperContext> options)
            : base(options)
        {
        }

        public DbSet<Entities.Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Article>()
                .Property(e => e.Uri)
                .IsRequired();
            modelBuilder.Entity<Entities.Article>()
                .HasIndex(e => e.Uri)
                .IsUnique();
        }
    }
}
