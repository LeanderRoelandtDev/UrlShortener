using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Infrastructure.Repositories
{
    internal class UrlRepository(UrlShortenerDbContext db) : IUrlRepository
    {
        public async Task<string> GetOriginalUrl(string shortUrl)
        {
            UrlEntity urlEntity = await db.UrlEntities.FirstOrDefaultAsync(url => url.ShortUrl == shortUrl);

            return urlEntity?.OriginalUrl;
        }

        public async Task<bool> IsDuplicate(string code)
        {
            return await db.UrlEntities.AnyAsync(url => url.ShortUrl == code);
        }

        public async Task<string> Create(string url, string shortUrl)
        {
            UrlEntity newUrl = new UrlEntity
            {
                OriginalUrl = url,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await db.UrlEntities.AddAsync(newUrl);
            int updatedRows = await db.SaveChangesAsync();
            if (updatedRows > 0)
            {
                return newUrl.ShortUrl;

            }

            throw new Exception("Something went wrong creating a UrlEntity");
        }
    }
}