using System;
using System.Threading.Tasks;

namespace Pieshop.Interfaces
{
    public interface IDistributedCacheService
    {
        Task<T> LoadFromDistributedCache<T>(string key, TimeSpan timeSpan, Func<Task<T>> operation);
    }
}
