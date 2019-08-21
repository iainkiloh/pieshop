using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pieshop.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pieshop.ControllerServices
{

    public class DistributedCacheService : IDistributedCacheService
    {
        protected readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> LoadFromDistributedCache<T>(string key, TimeSpan expiration, Func<Task<T>> operation)
        {
            var cachedData = await _distributedCache.GetAsync(key);
            if (cachedData == null)
            {
                var dataToCache = await operation();
                var serializedData = JsonConvert.SerializeObject(dataToCache);
                byte[] encodedByteArray = Encoding.UTF8.GetBytes(serializedData);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };
                await _distributedCache.SetAsync(key, encodedByteArray, cacheEntryOptions);
                return dataToCache;
            }
            else
            {
                byte[] encodedByteArray = await _distributedCache.GetAsync(key);
                var serializedData = Encoding.UTF8.GetString(encodedByteArray);
                var deserializedData = JsonConvert.DeserializeObject<T>(serializedData);
                return deserializedData;
            }
        }
 
    }
}
