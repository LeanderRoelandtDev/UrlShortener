using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace UrlShortener.Core.Interfaces.Repositories
{
    public interface IUrlRepository
    {
        bool Save(string url);
    }
}