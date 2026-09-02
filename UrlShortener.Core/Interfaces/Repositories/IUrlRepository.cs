using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UrlShortener.Core.Interfaces.Repositories
{
    public interface IUrlRepository
    {
        Task<bool> IsDuplicate(string code);
        Task<bool> SaveUrl(string url, string code);
    }
}