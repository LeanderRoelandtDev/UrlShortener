using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Infrastructure.Context
{
    public class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {
        internal DbSet<Url> Urls { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}