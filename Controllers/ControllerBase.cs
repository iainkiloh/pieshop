using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly IDistributedCache _distributedCache;

        public ControllerBase(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Base method for controllers to fetch data from distibuted redis cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual async Task<T> LoadFromDistributedCache<T>(string key, TimeSpan timeSpan, Func<Task<T>> operation)
        {
            var cachedData = await _distributedCache.GetAsync(key);
            if (cachedData == null)
            {
                var dbData = await operation();
                var serializedData = JsonConvert.SerializeObject(dbData);
                byte[] encodedByteArray = Encoding.UTF8.GetBytes(serializedData);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
                };
                await _distributedCache.SetAsync(key, encodedByteArray, cacheEntryOptions);
                return dbData;
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
