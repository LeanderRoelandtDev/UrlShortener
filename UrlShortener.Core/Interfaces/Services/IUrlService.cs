using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using UrlShortener.Dtos.Url.Request;
using UrlShortener.Dtos.Url.Response;
using UrlShortener.Models;

namespace UrlShortener.Core.Interfaces.Services
{
    public interface IUrlService
    {
        Task<string> GetOriginalUrl(string shortUrl);
        Task<List<UrlResponse>> GetAll(Guid userId);
        Task<UrlResponse> Create(CreateShortUrlRequest request, Guid userId);
        Task<UrlResponse> Edit(EditUrlEntityRequest request, Guid userId);
        Task Delete(string shortUrl, Guid userId);
    }
}