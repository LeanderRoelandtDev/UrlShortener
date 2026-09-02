using System;
using System.Collections.Generic;
using System.Text;
using UrlShortener.Dtos.Url.Request;

namespace UrlShortener.Core.Interfaces.Services
{
    public interface IUrlService
    {
        Task<bool> Save(CreateShortUrlRequest request);
    }
}