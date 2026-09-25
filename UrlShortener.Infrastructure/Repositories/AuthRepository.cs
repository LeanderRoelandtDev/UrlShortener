using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Infrastructure.Context;

namespace UrlShortener.Infrastructure.Repositories
{
    internal class AuthRepository(UrlShortenerDbContext db) : IAuthRepository
    {

    }
}