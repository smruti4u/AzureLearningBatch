using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisImplementation.Services
{
    public interface ICacheService
    {
        Task<T> GetData<T>(string key);

        Task SetData<T>(string key, T t, TimeSpan timeSpan);
    }
}
