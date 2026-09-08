
using System.CodeDom.Compiler;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Url.Request;

namespace UrlShortener.Core.Services
{
    internal class UrlService(IUrlRepository urlRepository) : IUrlService
    {
        public async Task<string> GetOriginalUrl(string shortUrl)
        {
            string originalUrl = await urlRepository.GetOriginalUrl(shortUrl);

            if (originalUrl == null)
            {
                throw ErrorException.NotFound("Couldnt not find any url with the provided code");
            }

            return originalUrl;
        }

        public async Task<string> Create(CreateShortUrlRequest request)
        {
            string generatedShortUrl = GenerateRandomCode();

            if (await urlRepository.IsDuplicate(generatedShortUrl))
            {
                generatedShortUrl = GenerateRandomCode();

                if (await urlRepository.IsDuplicate(generatedShortUrl))
                {
                    throw ErrorException.AlreadyExists("Unable to create an unique short url");
                }
            }

            return await urlRepository.Create(request.Url, generatedShortUrl);
        }

        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Range(0, 8)
                        .Select(_ => chars[Random.Shared.Next(chars.Length)])
                        .ToArray());
        }
    }
}