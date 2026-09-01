using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Infrastructure.Context
{
    public class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
    {

    }
}