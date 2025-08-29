using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shared.Application;

namespace Shared.Infrastructure
{
    internal sealed class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
 
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();
            if (expiry.HasValue) options.SetSlidingExpiration(expiry.Value);

            string? json = JsonConvert.SerializeObject(value);
            await _cache.SetStringAsync(key, json, options);
        }

        public Task RemoveAsync(string key)
            => _cache.RemoveAsync(key);
    }
}