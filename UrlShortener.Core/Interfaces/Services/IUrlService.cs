using System;
using System.Collections.Generic;
using System.Text;

namespace UrlShortener.Core.Interfaces.Services
{
    public interface IUrlService
    {
        bool Save(string url);
    }
}