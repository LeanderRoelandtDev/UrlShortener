using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Infrastructure.Context
{
    public class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {
        internal DbSet<UrlEntity> UrlEntities{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlEntity>()
                .HasIndex(x => x.ShortUrl)
                .IsUnique();
        }
    }
}