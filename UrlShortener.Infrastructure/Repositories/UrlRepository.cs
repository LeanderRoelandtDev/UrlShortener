using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;
using UrlEntity = UrlShortener.Infrastructure.DomainModels.UrlEntity;


namespace UrlShortener.Infrastructure.Repositories
{
    internal class UrlRepository(UrlShortenerDbContext db) : IUrlRepository
    {
        public async Task<string> GetOriginalUrl(string shortUrl)
        {
            UrlEntity urlEntity = await db.UrlEntities.FirstOrDefaultAsync(url => url.ShortUrl == shortUrl);

            return urlEntity?.OriginalUrl;
        }

        public async Task<List<Models.UrlEntity>> GetAll(Guid userId)
        {
            return await db.UrlEntities.AsNoTracking()
                .Where(url => url.ApplicationUserId == userId)
                .Select(url => new Models.UrlEntity
                {
                    Id = url.Id,
                    ShortUrl = url.ShortUrl,
                    OriginalUrl = url.OriginalUrl,
                    CreatedAt = url.CreatedAt,
                    UpdatedAt = url.UpdatedAt,
                    ClickCount = url.ClickCount,
                    UserId = url.ApplicationUserId
                }).ToListAsync();
        }

        public async Task<bool> IsDuplicate(string code)
        {
            return await db.UrlEntities.AnyAsync(url => url.ShortUrl == code);
        }

        public async Task<string> Create(string url, string shortUrl, Guid userId)
        {
            UrlEntity newUrl = new UrlEntity
            {
                OriginalUrl = url,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = userId
            };

            await db.UrlEntities.AddAsync(newUrl);
            int updatedRows = await db.SaveChangesAsync();
            if (updatedRows > 0)
            {
                return newUrl.ShortUrl;

            }

            throw new Exception("Something went wrong creating a UrlEntity");
        }


        public async Task<Models.UrlEntity?> GetByShortUrl(string shortUrl, Guid userId)
        {
            UrlEntity? entity = await db.UrlEntities.FirstOrDefaultAsync(url => url.ShortUrl == shortUrl && url.ApplicationUserId == userId);

            if (entity is null)
            {
                return null;
            }

            return new Models.UrlEntity
            {
                Id = entity.Id,
                OriginalUrl = entity.OriginalUrl,
                ShortUrl = entity.ShortUrl,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                UserId = entity.ApplicationUserId
            };
        }


        public async Task Update(Models.UrlEntity urlEntity)
        {
            UrlEntity entity = await db.UrlEntities.FirstAsync(x => x.Id == urlEntity.Id);

            entity.OriginalUrl = urlEntity.OriginalUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            int updatedRows = await db.SaveChangesAsync();

            if (updatedRows == 0)
            {
                throw new Exception("Something went wrong updating a UrlEntity");
            }
        }


        public async Task Delete(Models.UrlEntity urlEntity)
        {
            UrlEntity entity = await db.UrlEntities.FirstAsync(x => x.Id == urlEntity.Id);

            db.UrlEntities.Remove(entity);

            int deletedRows = await db.SaveChangesAsync();

            if (deletedRows == 0)
            {
                throw new Exception("Something went wrong deleting a UrlEntity");
            }
        }
    }
}