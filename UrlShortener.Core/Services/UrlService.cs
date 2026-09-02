
using System.CodeDom.Compiler;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Dtos.Url.Request;

namespace UrlShortener.Core.Services
{
    internal class UrlService(IUrlRepository urlRepository) : IUrlService
    {
        public async Task<bool> Save(CreateShortUrlRequest request)
        {
            string generatedCode = GenerateRandomCode();

            if (await urlRepository.IsDuplicate(generatedCode))
            {
                generatedCode = GenerateRandomCode();

                if (await urlRepository.IsDuplicate(generatedCode))
                {
                    throw new InvalidOperationException("Failed to generate a unique short code.");
                }
            }

            await urlRepository.SaveUrl(request.Url, generatedCode);

            //Implements eror handling
            return true;
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