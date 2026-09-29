using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Infrastructure.Context
{
    internal class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : IdentityUserContext<ApplicationUser, Guid>(options)
    {
        internal DbSet<UrlEntity> UrlEntities{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlEntity>()
                .HasIndex(x => x.ShortUrl)
                .IsUnique();
        }
    }
}