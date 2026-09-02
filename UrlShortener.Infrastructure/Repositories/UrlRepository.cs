using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Infrastructure.Repositories
{
    internal class UrlRepository(UrlShortenerDbContext db) : IUrlRepository
    {
        public async Task<bool> IsDuplicate(string code)
        {
            return await db.Urls.AnyAsync(url => url.Code == code);
        }

        public async Task<bool> SaveUrl(string url, string code)
        {
            Url newUrl = new Url
            {
                OriginalUrl = url,
                Code = code,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await db.Urls.AddAsync(newUrl);
            return await db.SaveChangesAsync() > 0;
        }
    }
}