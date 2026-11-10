using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Exceptions;
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

        public async Task<Models.UrlEntity> Create(string url, string shortUrl, Guid userId)
        {
            UrlEntity newUrl = new UrlEntity
            {
                OriginalUrl = url,
                ShortUrl = shortUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = userId
            };

            try
            {
                await db.UrlEntities.AddAsync(newUrl);
                await db.SaveChangesAsync();

                return new Models.UrlEntity
                {
                    Id = newUrl.Id,
                    ShortUrl = newUrl.ShortUrl,
                    OriginalUrl = newUrl.OriginalUrl,
                    CreatedAt = newUrl.CreatedAt,
                    UpdatedAt = newUrl.UpdatedAt,
                    ClickCount = newUrl.ClickCount,
                    UserId = newUrl.ApplicationUserId
                };
            }
            catch (DbUpdateException)
            {
                throw ErrorException.AlreadyExists("Unable to create a unique short url");
            }
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


        public async Task<Models.UrlEntity> Update(Models.UrlEntity urlEntity)
        {
            UrlEntity entity = await db.UrlEntities.FirstAsync(x => x.Id == urlEntity.Id);

            entity.OriginalUrl = urlEntity.OriginalUrl;
            entity.UpdatedAt = DateTime.UtcNow;


            try
            {
                await db.SaveChangesAsync();

                return new Models.UrlEntity
                {
                    Id = entity.Id,
                    ShortUrl = entity.ShortUrl,
                    OriginalUrl = entity.OriginalUrl,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt,
                    ClickCount = entity.ClickCount,
                    UserId = entity.ApplicationUserId
                };

            }
            catch (DbUpdateException)
            {
                throw ErrorException.AlreadyExists("Unable to update a unique short url");
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