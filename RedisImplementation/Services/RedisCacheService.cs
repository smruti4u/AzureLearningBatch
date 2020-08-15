using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisImplementation.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase database;

        private const string ConnectionString = "cachestore.redis.cache.windows.net:6380,password=as3ejtlDQ19J4whEk7yO3TYCLeneu5QqSaUy49DAEvQ=,ssl=True,abortConnect=False";
        public RedisCacheService()
        {

            database = GetDataBase(ConnectionString);
        }

        private IDatabase GetDataBase(string connectionString)
        {
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(connectionString);

            });

            return lazyConnection.Value.GetDatabase();
        }

        public async Task<T> GetData<T>(string key)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));

            var result = await database.StringGetAsync(key).ConfigureAwait(false);

            return (result == RedisValue.Null) ? default(T) : JsonConvert.DeserializeObject<T>(result);
        }

        public async Task SetData<T>(string key, T t, TimeSpan timeSpan)
        {
            key = key ?? throw new ArgumentNullException(nameof(key));
            t = t ?? throw new ArgumentNullException(nameof(t));

            var json = JsonConvert.SerializeObject(t);
            await database.StringSetAsync(key, json, timeSpan).ConfigureAwait(false);
        }
    }
}
