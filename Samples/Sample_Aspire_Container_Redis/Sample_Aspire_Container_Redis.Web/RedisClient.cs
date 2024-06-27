using Microsoft.Extensions.Caching.Distributed;

namespace Sample_Aspire_Container_Redis.Web;

public class RedisClient(IDistributedCache cache)
{
    public async Task<int> GetCounterAsync(string key)
    {
        // redis-cli
        // hget counter data
        var value = await cache.GetStringAsync(key);
        return value == null ? 0 : int.Parse(value);
    }

    public async Task SetCounterAsync(string key, int value)
    {
        await cache.SetStringAsync(key, value.ToString());
    }
}

