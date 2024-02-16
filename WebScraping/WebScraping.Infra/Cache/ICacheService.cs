using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Cache
{
    internal interface ICacheService
    {
        T Get<T>(String key);
        void Set<T> (string key, T value);

        void Remove(string key);
    }
}
