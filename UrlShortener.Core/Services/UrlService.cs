
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;

namespace UrlShortener.Core.Services
{
    internal class UrlService(IUrlRepository urlRepository) : IUrlService
    {
        public bool Save(string url)
        {
            urlRepository.Save(url);
            return true;
        }
    }
}