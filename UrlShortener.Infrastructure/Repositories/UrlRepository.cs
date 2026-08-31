using System;
using System.Collections.Generic;
using System.Text;
using UrlShortener.Core.Interfaces.Repositories;

namespace UrlShortener.Infrastructure.Repositories
{
    internal class UrlRepository : IUrlRepository
    {
        public bool Save(string url)
        {
            return true;
        }
    }
}