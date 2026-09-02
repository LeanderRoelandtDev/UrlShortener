using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UrlShortener.Core.Interfaces.Repositories
{
    public interface IUrlRepository
    {
        Task<string> GetOriginalUrl(string shortUrl);
        Task<bool> IsDuplicate(string shortUrl);
        Task<string> Create(string url, string shortUrl);
    }
}